using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BossMonster : MonoBehaviour
{
    [SerializeField] public int health = 375;
    [SerializeField] private int maxHealth;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private int attackDamage = 20;
    [SerializeField] private float attackRange = 4f;
    [SerializeField] private float aggroRange = 100f;
    public GameObject Victory;
    [SerializeField] private float attackCooldown = 2f; // Added attack cooldown
    PlayerHealth playerHealth;
     public Image xxx;
    private bool isDead = false;
    
    [SerializeField] private Animator animator;
    
    private Transform playerTransform;
    private bool isAttacking = false;
    private bool canAttack = true;
    
    void Start()
    {
        AudioManager.instance.Play("Spwn");
        // Find the player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            playerHealth = playerTransform.GetComponent<PlayerHealth>();
        }
        xxx.enabled = true;
        // Play spawn animation/sound if you have one
        AudioManager.instance.Play("BossSpawn");
        
        // Optional: Add screen shake or other effects for dramatic entrance
    }
    
    void Update()
    {
        if (playerTransform == null || isDead) return;
        
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        
        // If player is within aggro range
        if (distanceToPlayer <= aggroRange)
        {
            // If in attack range and not currently attacking and can attack
            if (distanceToPlayer <= attackRange && !isAttacking && canAttack)
            {
                StartCoroutine(AttackPlayer());
            }
            // Otherwise move towards player if not attacking
            else if (!isAttacking)
            {
                MoveTowardsPlayer();
            }
        }
        else
        {
            // Player out of range, return to idle/moving false
            if (animator != null)
            {
                animator.SetBool("isMoving", false);
            }
        }
    }
    
    private void MoveTowardsPlayer()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        // Face the player
        transform.LookAt(new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z));

        // Play movement animation
        if (animator != null)
        {
            animator.SetBool("isMoving", true);
        }
    }
      private void UpdateHealthBar()
    {
        
            // Update the fill amount (0 to 1)
            xxx.fillAmount = (float)health / maxHealth;
        
    }
    private IEnumerator AttackPlayer()
    {
        isAttacking = true;
        canAttack = false;
        
        // Stop moving
        if (animator != null)
        {
            animator.SetBool("isMoving", false);
            animator.SetTrigger("attack");
        }
        
        // Wait for animation to play (adjust time to match your animation length)
        yield return new WaitForSeconds(1.5f);
        
        // Deal damage to player if still in range
        if (playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            if (distanceToPlayer <= attackRange)
            {
              Debug.Log("Boss attacked player for " + attackDamage + " damage!");
                    playerHealth.TakeDamage(attackDamage); 
                    
                
                                   

            }
        }
        
        // Explicitly set back to moving if player still in range but outside attack range
        isAttacking = false;
        
        if (playerTransform != null)
        {
            float currentDistance = Vector3.Distance(transform.position, playerTransform.position);
            if (currentDistance > attackRange && currentDistance <= aggroRange)
            {
                animator.SetBool("isMoving", true);
            }
        }
        
        // Attack cooldown
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
    
    public void TakeDamage(int damage)
    {
        health -= damage;
        UpdateHealthBar();
        if (health <= 0)
        {
            Die();
        }
        else
        {
            // Play hit reaction
            if (animator != null)
            {
                animator.SetTrigger("hit");
                Debug.Log("Player attacked boss for " + damage + " damage!");
            }
            AudioManager.instance.Play("BossDamage");
        }
    }
    
    private void Die()
    {
        isDead = true;
       playerHealth.TakeDamage(1);     

        // Stop all movement and attacks
        StopAllCoroutines();
        isAttacking = false;
        
        // Play death animation
        if (animator != null)
        {
            animator.SetBool("isMoving", false);
            animator.SetTrigger("die");
        }
        
        AudioManager.instance.Play("BossDie");
        
        // Disable collider and components
        GetComponent<Collider>().enabled = false;
        
        // Optional: Drop loot
         Victory.SetActive(true);
        
        // Destroy after delay to allow animation to play
        Destroy(gameObject, 3f);

    }
}