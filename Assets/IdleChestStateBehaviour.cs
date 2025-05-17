using UnityEngine;
using UnityEngine.AI;

public class IdleChestStateBehaviour : StateMachineBehaviour
{
    private float closeDetectionRadius = 7f;
    // Animator parameter name for HasTaunted flag
    public string hasTauntedBoolName = "HasTaunted"; 
    // Animator parameter name for player in close range
    public string playerInCloseRangeBoolName = "PlayerInCloseRange";

    private Transform playerTransform;
    private NavMeshAgent navMeshAgent;
    private bool _hasTaunted;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null) playerTransform = playerObj.transform;
        }
        if (navMeshAgent == null) navMeshAgent = animator.GetComponent<NavMeshAgent>();

        navMeshAgent?.SetDestination(animator.transform.position); // Stop movement
        navMeshAgent?.Stop();

        // Get the current HasTaunted status from Animator
        _hasTaunted = animator.GetBool(hasTauntedBoolName);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playerTransform == null || _hasTaunted) 
        {
            // If player doesn't exist or monster has already taunted, stay idle (or handle differently if needed)
            // If already taunted, it should ideally be in Run state, not Idle.
            // This check prevents transitioning if it somehow got here after taunting.
            return; 
        }

        float distanceToPlayer = Vector3.Distance(animator.transform.position, playerTransform.position);

        if (distanceToPlayer <= closeDetectionRadius)
        {
            animator.SetBool(playerInCloseRangeBoolName, true);
            Debug.Log("Distance: " + distanceToPlayer + " | Radius: " + closeDetectionRadius);

            
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Optionally reset the trigger if this state is exited for other reasons, 
        // though transitions should handle this.
        // animator.SetBool(playerInCloseRangeBoolName, false); 
    }
}
