using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private int hp = 100;
    public Animator animator;
    
    // Event that will be triggered when the chest is destroyed
    public delegate void ChestDestroyedEvent();
    public event ChestDestroyedEvent OnChestDestroyed;
    
    private bool isDestroyed = false;
    
    // Start is called before the first frame update
    void Start()
    {
        // Register this chest with the ChestManager
        if (ChestManager.instance != null)
        {
            ChestManager.instance.RegisterChest(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {

            if (!isDestroyed)
            {
                isDestroyed = true;
                AudioManager.instance.Play("Die");
                animator.SetTrigger("die");
                // GetComponent<Collider>().enabled = false;

                // Notify that this chest has been destroyed
                OnChestDestroyed?.Invoke();
            }
        }
        else
        {
            AudioManager.instance.Play("Damage");
            animator.SetTrigger("damage");
        }
    }
}