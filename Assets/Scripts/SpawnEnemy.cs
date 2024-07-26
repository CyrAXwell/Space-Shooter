using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;

    void Start()
    {
        
    }


    void Update()
    {
        if(Input.GetKeyDown("e"))
        {
            Instantiate(enemyPrefab);
        }
    }
}
