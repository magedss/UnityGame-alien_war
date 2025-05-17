using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // Required for NavMeshAgent

public class chase : StateMachineBehaviour
{
    public float chaseSpeed = 3.5f;       // Speed at which entity chases player
    public float updatePathInterval = 0.2f; // How often to recalculate path
    public string playerTag = "Player";   // Tag of the player object
    public string lostPlayerParameter = "isNearby"; // Parameter to set when player is lost
    public float maxChaseDistance = 15f;  // Maximum distance entity will chase before giving up
    
    private Transform playerTransform;
    private NavMeshAgent navMeshAgent;
    private float pathUpdateTimer;
    private Vector3 lastKnownPosition;
    private bool hasSetupAgent = false;
    
    // Called when this state begins
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Find the player
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        if (player != null)
        {
            playerTransform = player.transform;
            lastKnownPosition = playerTransform.position;
        }
        
        // Get or add the NavMeshAgent component
        navMeshAgent = animator.GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
        {
            // Try to add the component if it doesn't exist
            navMeshAgent = animator.gameObject.AddComponent<NavMeshAgent>();
        }
        
        if (navMeshAgent != null)
        {
            // Store original settings to restore on exit
            hasSetupAgent = true;
            
            // Configure the agent for chasing
            navMeshAgent.speed = chaseSpeed;
            navMeshAgent.isStopped = false;
            navMeshAgent.updateRotation = true;
            
            // Start moving toward the player
            if (playerTransform != null)
            {
                navMeshAgent.SetDestination(playerTransform.position);
            }
        }
        else
        {
            Debug.LogError("NavMeshAgent component could not be found or added to " + animator.gameObject.name);
        }
        
        // Reset the path update timer
        pathUpdateTimer = 0f;
    }
    
    // Called every frame while in this state
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (navMeshAgent == null || !hasSetupAgent) return;
        
        // Update the path to the player at the specified interval
        pathUpdateTimer -= Time.deltaTime;
        if (pathUpdateTimer <= 0f)
        {
            pathUpdateTimer = updatePathInterval;
            
            // If we have a player reference, update the path
            if (playerTransform != null)
            {
                // Update last known position
                lastKnownPosition = playerTransform.position;
                
                // Set the destination to the player's current position
                navMeshAgent.SetDestination(lastKnownPosition);
                
                // Check if we've chased too far
                float distanceFromStart = Vector3.Distance(animator.transform.position, lastKnownPosition);
                if (distanceFromStart > maxChaseDistance)
                {
                    // Player is too far away, give up chase
                    animator.SetBool(lostPlayerParameter, false);
                    Debug.Log("Chase abandoned - too far from target");
                }
            }
            else
            {
                // Try to find the player again
                GameObject player = GameObject.FindGameObjectWithTag(playerTag);
                if (player != null)
                {
                    playerTransform = player.transform;
                }
            }
        }
        
        // Check if we've reached the destination
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            if (navMeshAgent.hasPath && navMeshAgent.velocity.sqrMagnitude == 0f)
            {
                // We've reached the last known position but don't see the player
                // Check if we need to give up
                if (playerTransform == null)
                {
                    animator.SetBool(lostPlayerParameter, false);
                    Debug.Log("Chase abandoned - reached last known position");
                }
            }
        }
    }
    
    // Called when this state exits
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Stop the agent when exiting this state
        if (navMeshAgent != null && hasSetupAgent)
        {
            navMeshAgent.isStopped = true;
        }
    }
}