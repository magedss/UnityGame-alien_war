using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sense : StateMachineBehaviour
{
    public float detectionRadius = 6f;    // How far the entity can sense
    public string playerTag = "Player";   // Tag of the player object
    public string detectedParameter = "isNearby"; // Animator parameter to set when player is detected
    public string tauntParameter = "Taunt";
    public float tauntingRadius = 3f;

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
            
            // Check if player is NOT within detection radius
            bool isPlayerFar = distanceToPlayer > detectionRadius;
            bool taunt = distanceToPlayer <= tauntingRadius;

            // If player is far away, transition to idle state
            if (isPlayerFar)
            {
                // Set the parameter to false to indicate player is not nearby
                animator.SetBool(detectedParameter, false);
                
                // Debug message
                Debug.Log("Player out of range, returning to idle");
            }
            if (taunt)
            {
             animator.SetBool(tauntParameter, true);
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
    
    // Optional: Visualize the detection radius in the editor
    #if UNITY_EDITOR
    public void OnDrawGizmosSelected()
    {
        // We can't access the animator directly here, so we'll need to use the component this is attached to
        if (UnityEditor.Selection.activeGameObject != null && 
            UnityEditor.Selection.activeGameObject.GetComponent<Animator>() != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(UnityEditor.Selection.activeGameObject.transform.position, detectionRadius);
        }
    }
    #endif
}