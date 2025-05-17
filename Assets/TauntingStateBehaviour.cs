using UnityEngine;
using UnityEngine.AI;

public class TauntingStateBehaviour : StateMachineBehaviour
{
    public float tauntDuration = 3f;
    public string hasTauntedBoolName = "HasTaunted";
    public string tauntFinishedTriggerName = "TauntFinished";

    private Transform playerTransform;
    private NavMeshAgent navMeshAgent;
    private float tauntTimer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AudioManager.instance.Play("Rage");
        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null) playerTransform = playerObj.transform;
        }
        if (navMeshAgent == null) navMeshAgent = animator.GetComponent<NavMeshAgent>();
        
        navMeshAgent?.SetDestination(animator.transform.position); // Stop movement
        navMeshAgent?.Stop();
        FacePlayer(animator.transform);
        tauntTimer = 0f;
        // isTaunting flag (if needed for other systems) could be a local bool or another Animator param
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playerTransform == null) return;

        FacePlayer(animator.transform);
        tauntTimer += Time.deltaTime;

        if (tauntTimer >= tauntDuration)
        {
            animator.SetBool(hasTauntedBoolName, true); // POINT OF NO RETURN
            animator.SetTrigger(tauntFinishedTriggerName);
        }
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
