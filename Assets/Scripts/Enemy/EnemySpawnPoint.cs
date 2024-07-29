using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    //private Rigidbody2D rb;

    [SerializeField] private GameObject[] enemyPrefab;
    public int enemyID = 0;

    private float spawnTime = 1.5f;

    void Start()
    {
        //rb = GetComponent<Rigidbody2D>();
        StartCoroutine(SpawnEnemy(spawnTime, enemyPrefab[enemyID]));
    }


    private IEnumerator SpawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);
        Instantiate(enemy,transform.position, Quaternion.identity);
        Destroy(gameObject);

    }
}
