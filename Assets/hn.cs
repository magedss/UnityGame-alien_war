using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hn : MonoBehaviour
{
   [SerializeField] private GameObject bossMonsterPrefab;
   [SerializeField] private GameObject Victory1;
    [SerializeField] private BossMonster boss;

    // Start is called before the first frame update
    void Start()
    {
        boss =bossMonsterPrefab.GetComponent<BossMonster>(); 
Debug.Log("fddf " + " sssssssssss!");   
    }

    // Update is called once per frame
    void Update()
    {
       if (boss.health <= 0)
        {
        Victory1.SetActive(true);

        }
    }
}
