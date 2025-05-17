using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestManager : MonoBehaviour
{
    public static ChestManager instance;
    
    [SerializeField] private GameObject bossMonsterPrefab;
    [SerializeField] private Transform spawnPoint;
    
    private List<Chest> chests = new List<Chest>();
    private int destroyedChestsCount = 0;
    private bool bossSpawned = false;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        // Find all chests in the scene
        Chest[] scenechests = FindObjectsOfType<Chest>();
        
        foreach (Chest chest in scenechests)
        {
            RegisterChest(chest);
        }
    }
    
    public void RegisterChest(Chest chest)
    {
        if (!chests.Contains(chest))
        {
            chests.Add(chest);
            // Subscribe to the chest's death event
            chest.OnChestDestroyed += HandleChestDestroyed;
        }
    }
    
    private void HandleChestDestroyed()
    {
        destroyedChestsCount++;
        
        // Check if all chests are destroyed
        if (destroyedChestsCount >= chests.Count && !bossSpawned)
        {
            bossSpawned = true;
            SpawnBossMonster();
        }
    }
    
    private void SpawnBossMonster()
    {
        if (bossMonsterPrefab != null && spawnPoint != null)
        {
            Debug.Log("Spawning boss monster!");
            Instantiate(bossMonsterPrefab, spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            Debug.LogError("Boss monster prefab or spawn point not assigned in the ChestManager!");
        }
    }
}