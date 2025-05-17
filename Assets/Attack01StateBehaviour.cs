using UnityEngine;
using UnityEngine.AI;

public class Attack01StateBehaviour : StateMachineBehaviour
{
    // public float attackDuration = 1f; // Or driven by animation length
    // Add any specific parameters for this attack if needed (e.g., damage amount, effect prefabs)
    // These would be public and set in Inspector.

    private NavMeshAgent navMeshAgent;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (navMeshAgent == null) navMeshAgent = animator.GetComponent<NavMeshAgent>();
        
        // Stop movement during attack, or allow slight movement based on animation
        // navMeshAgent?.Stop(); 
        // Or navMeshAgent.velocity = Vector3.zero;

        // --- Actual Attack Logic Placeholder ---
        // Debug.Log("Performing Short Range Attack (Attack01)!");
        // Instantiate attack hitbox, play sound, apply damage, etc.
        // This logic would go here.
        // For example: animator.GetComponent<MonsterDamageDealer>()?.DealShortRangeDamage();
        // --- End Attack Logic Placeholder ---

        // Transition back to Run state is usually handled by Animator transition (e.g., "Has Exit Time")
        // or an animation event calling a method to set a trigger.
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // If attack has a specific duration not tied to animation length, handle here.
        // Or if it needs to track player during attack animation (e.g. a lunging attack)
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // navMeshAgent?.Resume(); // Resume movement if stopped
        animator.ResetTrigger("TriggerAttack01"); // Important to reset the trigger
    }
}
