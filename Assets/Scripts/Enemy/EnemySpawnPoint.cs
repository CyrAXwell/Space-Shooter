using System.Collections;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    private float spawnTime = 1.5f;

    public void Initialize(Enemy enemyPrefab)
    {
        StartCoroutine(SpawnEnemy(spawnTime, enemyPrefab));
    }

    private IEnumerator SpawnEnemy(float interval, Enemy enemy)
    {
        yield return new WaitForSeconds(interval);
        Instantiate(enemy.gameObject ,transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
