using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BulletProjectile : MonoBehaviour {
    [SerializeField] private Transform vfxHitGreen;
    [SerializeField] private Transform vfxHitRed;
    private int bulletDamage = 10;
    private Rigidbody bulletRigidbody;
   public float currentHealth=375;
     public Image healthBar; 

    private void Awake() {
        bulletRigidbody = GetComponent<Rigidbody>();
    }

    private void Start() {
        float speed = 60f;
        bulletRigidbody.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other) {
              Destroy(gameObject);
        if (other.GetComponent<Chest>() != null)
        {
            // Hit target
             other.GetComponent<Chest>().TakeDamage(bulletDamage);
           

            Instantiate(vfxHitGreen, transform.position, Quaternion.identity);
        }
         if (other.GetComponent<BossMonster>() != null)
        {
             other.GetComponent<BossMonster>().TakeDamage(bulletDamage);
            Debug.Log("  player attacked forrrrr " + 10 + " damage!");
            currentHealth -= 10;
                      
        healthBar.fillAmount = (float)currentHealth / 375;

            Instantiate(vfxHitGreen, transform.position, Quaternion.identity);
        }
        else
        {
            // Hit something else
            Instantiate(vfxHitRed, transform.position, Quaternion.identity);
        }
        
    }

}