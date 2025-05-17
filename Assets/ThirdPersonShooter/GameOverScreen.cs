using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading.Tasks;


public class GameOverScreen : MonoBehaviour
{
    public float delayBeforeRestart = 3f;
    public Button restartButton; // Optional restart button
    
    void Start()
    {
        // Hide initially (should be handled by PlayerHealth, but just in case)
        gameObject.SetActive(false);
        
        // Setup restart button if it exists
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }
        
        // Start automatic restart countdown when screen is shown
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(RestartAfterDelay());
        }
    }
    
    // Called when the game over screen is activated
    void OnEnable()
    {
        // Start the restart countdown
        StartCoroutine(RestartAfterDelay());
    }
    
    // Automatic restart after delay
    IEnumerator RestartAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeRestart);
        RestartGame();
    }
    
    // Restart the current scene

public void RestartGame()
{
    // Load scene 1 additively and wait until done
     SceneManager.LoadScene(1);

    // Optionally unload scene 0 after loading scene 1 (depends on your need)
     SceneManager.UnloadScene(0);
        SceneManager.LoadScene(0, LoadSceneMode.Additive);
    // Or, if you want to just reload scene 0, do it asynchronously:
    /// SceneManager.LoadScene(0, LoadSceneMode.Single);
}
}