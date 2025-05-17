using UnityEngine;
using UnityEngine.AI;

public class RunStateBehaviour : StateMachineBehaviour
{
    public float movementSpeed = 4f;
    public float shortAttackRange = 2f;
    public float longAttackRange = 8f;
    public float shortAttackCooldown = 1.5f;
    public float longAttackCooldown = 2.5f;

    public string triggerAttack01Name = "TriggerAttack01";
    public string triggerAttack02Name = "TriggerAttack02";
    // HasTaunted should be checked by transitions *into* this state, ensuring it only runs if true.

    private Transform playerTransform;
    private NavMeshAgent navMeshAgent;
    private float currentShortAttackCooldown = 0f;
    private float currentLongAttackCooldown = 0f;
    PlayerHealth playerHealth;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AudioManager.instance.Play("Run");
        if (playerTransform == null) 
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            

            if (playerObj != null) playerTransform = playerObj.transform;
            playerHealth = playerTransform.GetComponent<PlayerHealth>();
        }
        if (navMeshAgent == null) navMeshAgent = animator.GetComponent<NavMeshAgent>();

        if (navMeshAgent != null) 
        {
            navMeshAgent.speed = movementSpeed;
            navMeshAgent.isStopped = false; // Ensure it can move
            navMeshAgent.Resume();
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AudioManager.instance.Play("Bite");
        if (playerTransform == null || navMeshAgent == null) return;

        navMeshAgent.SetDestination(playerTransform.position);

        if (currentShortAttackCooldown > 0) currentShortAttackCooldown -= Time.deltaTime;
        if (currentLongAttackCooldown > 0) currentLongAttackCooldown -= Time.deltaTime;

        float distanceToPlayer = Vector3.Distance(animator.transform.position, playerTransform.position);

        if (distanceToPlayer <= shortAttackRange && currentShortAttackCooldown <= 0)
        {
            animator.SetTrigger(triggerAttack01Name);
            currentShortAttackCooldown = shortAttackCooldown; 
            playerHealth.TakeDamage(10); 
             //AudioManager.instance.Play("Bite");
            // Attack01StateBehaviour will handle actual attack logic/animation
        }
        else if (distanceToPlayer <= longAttackRange && distanceToPlayer > shortAttackRange && currentLongAttackCooldown <= 0)
        {
            animator.SetTrigger(triggerAttack02Name);
            currentLongAttackCooldown = longAttackCooldown;
            playerHealth.TakeDamage(10); 
           // AudioManager.instance.Play("Bite");
            // Attack02StateBehaviour will handle actual attack logic/animation
        }
    }
    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // If this state is somehow exited (e.g. player dies, game ends), stop the agent.
        // For perpetual chase, this state might not have an explicit exit other than to attack states.
        // navMeshAgent?.Stop(); 
    }
}
