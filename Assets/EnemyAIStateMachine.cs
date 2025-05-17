using UnityEngine;
using System.Collections;

public class EnemyAIStateMachine : MonoBehaviour
{
    // States
    public enum EnemyState
    {
        IdleChest,
        SenseSomethingRPT,
        Taunting,
        Attack01,
        Run,
        Attack02
    }

    // Current state
    public EnemyState currentState = EnemyState.IdleChest;

    // References
    public Transform player;
    public Animator animator;

    // Parameters for state transitions
    [Header("Detection Settings")]
    public float senseDistance = 8f;    // Distance to transition from Idle to Sense
    public float tauntDistance = 4f;    // Distance to transition from Sense to Taunt
    
    [Header("Attack Settings")]
    public float attack01Distance = 2f;   // Distance for Attack01 (close range)
    public float attack02Distance = 4f;   // Distance for Attack02 (longer range)
    public float chaseSpeed = 3f;         // Speed at which enemy chases player
    public float attackCooldown = 1f;     // Time between attacks

    // Animation parameter names
    private readonly string animParamIdle = "IsIdle";
    private readonly string animParamSense = "IsSensing";
    private readonly string animParamTaunt = "IsTaunting";
    private readonly string animParamAttack01 = "IsAttack01";
    private readonly string animParamAttack02 = "IsAttack02";
    private readonly string animParamRun = "IsRunning";

    // State flags
    private bool isRageMode = false;
    private bool hasEnteredChase = false;
    private float lastAttackTime = -10f;
    private bool tauntComplete = false;
    private bool isAttacking = false;

    void Start()
    {
        // Initialize references
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (animator == null)
            animator = GetComponent<Animator>();

        // Initialize with Idle state
        ChangeState(EnemyState.IdleChest);
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case EnemyState.IdleChest:
                IdleChestBehavior(distanceToPlayer);
                break;

            case EnemyState.SenseSomethingRPT:
                SenseBehavior(distanceToPlayer);
                break;

            case EnemyState.Taunting:
                TauntingBehavior(distanceToPlayer);
                break;

            case EnemyState.Attack01:
                Attack01Behavior(distanceToPlayer);
                break;

            case EnemyState.Run:
                RunBehavior(distanceToPlayer);
                break;

            case EnemyState.Attack02:
                Attack02Behavior(distanceToPlayer);
                break;
        }
    }

    // State behaviors
    void IdleChestBehavior(float distanceToPlayer)
    {
        if (hasEnteredChase)
        {
            ChangeState(EnemyState.Run);
            return;
        }

        if (distanceToPlayer <= senseDistance)
        {
            ChangeState(EnemyState.SenseSomethingRPT);
        }
    }

    void SenseBehavior(float distanceToPlayer)
    {
        if (hasEnteredChase)
        {
            ChangeState(EnemyState.Run);
            return;
        }

        LookAtPlayer();

        if (distanceToPlayer > senseDistance)
        {
            ChangeState(EnemyState.IdleChest);
        }
        else if (distanceToPlayer <= tauntDistance)
        {
            ChangeState(EnemyState.Taunting);
        }
    }

    void TauntingBehavior(float distanceToPlayer)
    {
        LookAtPlayer();
        isRageMode = true;

        if (tauntComplete)
        {
            hasEnteredChase = true;
            ChangeState(EnemyState.Run);
        }
    }

    void Attack01Behavior(float distanceToPlayer)
    {
        LookAtPlayer();
        hasEnteredChase = true;

        if (!isAttacking)
        {
            lastAttackTime = Time.time;
            isAttacking = true;
        }
    }

    void RunBehavior(float distanceToPlayer)
    {
        hasEnteredChase = true;

        // Move toward player if not in attack range
        if (distanceToPlayer > attack01Distance)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, 
                player.position, 
                chaseSpeed * Time.deltaTime
            );
        }

        LookAtPlayer();
        
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            if (distanceToPlayer <= attack01Distance)
            {
                ChangeState(EnemyState.Attack01);
            }
            else if (distanceToPlayer <= attack02Distance)
            {
                ChangeState(EnemyState.Attack02);
            }
        }
    }

    void Attack02Behavior(float distanceToPlayer)
    {
        LookAtPlayer();
        hasEnteredChase = true;

        if (!isAttacking)
        {
            lastAttackTime = Time.time;
            isAttacking = true;
        }
    }

    void ChangeState(EnemyState newState)
    {
        ExitState(currentState);
        currentState = newState;
        EnterState(newState);
        Debug.Log($"Enemy state changed to: {newState}");
    }

    void EnterState(EnemyState state)
    {
        // Reset all animation parameters
        animator.SetBool(animParamIdle, false);
        animator.SetBool(animParamSense, false);
        animator.SetBool(animParamTaunt, false);
        animator.SetBool(animParamAttack01, false);
        animator.SetBool(animParamAttack02, false);
        animator.SetBool(animParamRun, false);

        // Reset state-specific flags
        isAttacking = false;
        
        switch (state)
        {
            case EnemyState.IdleChest:
                animator.SetBool(animParamIdle, true);
                break;

            case EnemyState.SenseSomethingRPT:
                animator.SetBool(animParamSense, true);
                break;

            case EnemyState.Taunting:
                animator.SetBool(animParamTaunt, true);
                tauntComplete = false;
                break;

            case EnemyState.Attack01:
                animator.SetBool(animParamAttack01, true);
                break;

            case EnemyState.Run:
                animator.SetBool(animParamRun, true);
                break;

            case EnemyState.Attack02:
                animator.SetBool(animParamAttack02, true);
                break;
        }
    }

    void ExitState(EnemyState state)
    {
        // Cleanup logic if needed
    }

    void LookAtPlayer()
    {
        if (player != null)
        {
            Vector3 direction = player.position - transform.position;
            direction.y = 0;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(direction),
                    5f * Time.deltaTime
                );
            }
        }
    }

    // Animation event handlers
    public void OnTauntComplete()
    {
        tauntComplete = true;
    }

    public void OnAttackComplete()
    {
        isAttacking = false;
        ChangeState(EnemyState.Run);
    }

    // Debug visualization

}