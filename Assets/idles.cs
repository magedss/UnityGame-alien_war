using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class idles : StateMachineBehaviour
{
    public float detectionRadius = 8f;    // How far the entity can sense
    public string playerTag = "Player";   // Tag of the player object
    public string detectedParameter = "isNearby"; // Animator parameter to set when player is detected
    
    private Transform playerTransform;
    private bool playerDetected = false;
    
    // Called when a state begins
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Find the player once when entering the state
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    // Called every frame while in this state
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // If we have a reference to the player, check distance
        if (playerTransform != null)
        {
            // Get the entity's position (the GameObject the Animator is attached to)
            Transform entityTransform = animator.transform;
            
            // Calculate distance between entity and player
            float distanceToPlayer = Vector3.Distance(entityTransform.position, playerTransform.position);
            
            // Check if player is within detection radius
            bool isPlayerNear = distanceToPlayer <= detectionRadius;
            
            // If player is nearby, transition from idle state
            if (isPlayerNear)
            {
                // Set the parameter to true to indicate player is nearby
                animator.SetBool(detectedParameter, true);
                
                // Debug message
                Debug.Log("Player detected, leaving idle state!");
            }
        }
    }

    // Called when a state exits
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Optional: Reset detection when exiting this state
        // playerDetected = false;
        // animator.SetBool(detectedParameter, false);
    }

    // The following methods are left empty as they're not needed for this functionality
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
    

}