using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public GameObject gameOverScreen;
    public GameObject Victory;
    public Image healthBar; // Your horizontal health bar image
    
    void Start()
    {
        currentHealth = maxHealth;
       
        
        
    }
    
    public void TakeDamage(int damage)
    {
        if (damage == 1)
        {
          Victory.SetActive(true);

        }
        currentHealth -= damage;
        
        // Make sure health doesn't go below 0
        if (currentHealth < 0)
            currentHealth = 0;
            
        UpdateHealthBar();
        
        // Check if player died
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // Play damage animation/sound if you have one
         //   AudioManager.instance.Play("PlayerDamage");
        }
    }
    
    private void UpdateHealthBar()
    {
        
            // Update the fill amount (0 to 1)
            healthBar.fillAmount = (float)currentHealth / maxHealth;
        
    }
    
    private void Die()
    {
        // Play death sound if you have one
       // AudioManager.instance.Play("PlayerDeath");
                            Debug.Log("fddf " + " damage!");
        StartCoroutine(ShowGameOverScreen());



    }
        private IEnumerator ShowGameOverScreen()
    {
        // Wait a bit before showing game over screen
        yield return new WaitForSeconds(0f);
        
        // Show the game over screen
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }
    }
   
}