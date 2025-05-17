using UnityEngine;
using UnityEngine.AI;

public class SenseSomethingRPTStateBehaviour : StateMachineBehaviour
{
   private float closeDetectionRadius = 7f; // Should match Idle for consistency on exit
   private float tauntDetectionRadius = 4f;
    public string hasTauntedBoolName = "HasTaunted";
    public string playerInTauntRangeBoolName = "PlayerInTauntRange";
    public string playerInCloseRangeBoolName = "PlayerInCloseRange"; // For transitioning back to Idle

    private Transform playerTransform;
    private bool _hasTaunted;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
     

        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null) playerTransform = playerObj.transform;
        }
        _hasTaunted = animator.GetBool(hasTauntedBoolName);
        FacePlayer(animator.transform);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playerTransform == null || _hasTaunted) return;

        FacePlayer(animator.transform);
        float distanceToPlayer = Vector3.Distance(animator.transform.position, playerTransform.position);

        if (distanceToPlayer <= tauntDetectionRadius)
        {
            animator.SetBool(playerInTauntRangeBoolName, true);
        }
        else if (distanceToPlayer > closeDetectionRadius) // Player moved out of even the wider sense range
        {
            animator.SetBool(playerInCloseRangeBoolName, false); // Trigger transition back to Idle
        }
    }
    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // animator.SetBool(playerInTauntRangeBoolName, false); // Reset on exit
    }

    void FacePlayer(Transform monsterTransform)
    {
        if (playerTransform != null)
        {
            Vector3 direction = (playerTransform.position - monsterTransform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            monsterTransform.rotation = Quaternion.Slerp(monsterTransform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }
}

