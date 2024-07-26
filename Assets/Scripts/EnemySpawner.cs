using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] private GameObject enemyMarkerPrefab;

    [SerializeField] private float spawnTimer;

    [SerializeField] private float numOfEnemy;

    [SerializeField] private Transform redZone;
    private float redZoneL;
    private float redZoneR;
    private float redZoneTop;
    private float redZoneBottom;
    [SerializeField] private Transform blueZone;
    private float blueZoneL;
    private float blueZoneR;
    private float blueZoneTop;
    private float blueZoneBottom;
    [SerializeField] private Transform greenZone;
    private float greenZoneL;
    private float greenZoneR;
    private float greenZoneTop;
    private float greenZoneBottom;

    [SerializeField] private int[] enemyID = {0, 1, 2, 3, 4, 5};
    private int[] enemySpawnProbability = {0, 0, 0, 0, 0, 0};
    private int[] cumulativeProbability = {0, 0, 0, 0, 0, 0};

    //private int wave = 1;


    private float timer = 0f;
    private float delay;

    void Start()
    {
        timer = spawnTimer - 2f;
        //StartCoroutine(SpawnEnemy(spawnTimer, enemyPrefab[0]));
        redZoneL = redZone.position.x - redZone.localScale.x / 2;
        redZoneR = redZone.position.x + redZone.localScale.x / 2;
        redZoneTop = redZone.position.y + redZone.localScale.y / 2;
        redZoneBottom = redZone.position.y - redZone.localScale.y / 2;

        blueZoneL = blueZone.position.x - blueZone.localScale.x / 2;
        blueZoneR = blueZone.position.x + blueZone.localScale.x / 2;
        blueZoneTop = blueZone.position.y + blueZone.localScale.y / 2;
        blueZoneBottom = blueZone.position.y - blueZone.localScale.y / 2;

        greenZoneL = greenZone.position.x - greenZone.localScale.x / 2;
        greenZoneR = greenZone.position.x + greenZone.localScale.x / 2;
        greenZoneTop = greenZone.position.y + greenZone.localScale.y / 2;
        greenZoneBottom = greenZone.position.y - greenZone.localScale.y / 2;
        //Debug.Log(redZoneL);
        UpdateProbability(1);
        //GetProbability(enemySpawnProbability);
    }

    void FixedUpdate()
    {
        timer -= Time.fixedDeltaTime;
        if(timer <= 0)
        {
            timer = spawnTimer;
            SpawnEnemy(numOfEnemy);
            
        }

    }

    public void UpdateProbability(int wave)
    {
        switch(wave)
        {
            case 1: enemySpawnProbability = new int[] {8000, 2000, 0, 0, 0, 0}; break;          //{8000, 2000, 0, 0, 0, 0}
                   
            case 2: enemySpawnProbability = new int[] {8000, 2000, 0, 0, 0, 0}; break;          //{7000, 3000, 0, 0, 0, 0}
                
            case 3: enemySpawnProbability = new int[] {6200, 2000, 1800, 0, 0, 0}; break;       //{6000, 2500, 1500, 0, 0, 0}
                
            case 4: enemySpawnProbability = new int[] {6200, 2000, 1800, 0, 0, 0}; break;       //{5500, 2500, 2000, 0, 0, 0}
                
            case 5: enemySpawnProbability = new int[] {4700, 2000, 1800, 0, 1500, 0}; break;    //{4000, 2500, 2000, 0, 1500, 0}
                
            case 6: enemySpawnProbability = new int[] {4700, 2000, 1800, 0, 1500, 0}; break;    //{4000, 2500, 2000, 0, 1500, 0}
                   
            case 7: enemySpawnProbability = new int[] {4700, 2000, 1800, 0, 1500, 0}; break;    //{4000, 2500, 2000, 0, 1500, 0}
                      
            case 8: enemySpawnProbability = new int[] {3600, 2000, 1800, 0, 1500, 1100}; break; //{3000, 2500, 2000, 0, 1500, 1000}

            case 9: enemySpawnProbability = new int[] {3600, 2000, 1800, 0, 1500, 1100}; break; //{3000, 2400, 2000, 0, 1500, 1100}
                 
            case 10: enemySpawnProbability = new int[] {3600, 2000, 1800, 0, 1500, 1100}; break;//{3000, 2400, 2000, 0, 1500, 1100}
                 
            case >= 11: enemySpawnProbability = new int[] {3900, 2000, 1800, 700, 1100, 500}; break; //{2500, 2000, 1800, 1100, 1500, 1100}
                       
        }
        if(wave == 5)
        {
            numOfEnemy ++;    
        }
        GetProbability(enemySpawnProbability);
        timer = spawnTimer - 2f;
    }

    int GetEnemyByProbability(int[] probability)
    {
        int randomNumber = Random.Range(0, 10001);
        for (int i = 0; i < probability.Length; i++)
        {
            if (randomNumber <= cumulativeProbability[i])
            {
                return i;
            }
        }

        return -1;
    }

    void GetProbability(int[] enemyProbability)
    {
        int probabilitySum = 0;
        for (int i = 0; i < enemyProbability.Length; i++)
        {
            probabilitySum += enemyProbability[i];
            cumulativeProbability[i] = probabilitySum;
        }
    }

    void SpawnEnemy(float numOfEnemy)
    {
        for(var i = 1; i <= numOfEnemy; i++)
        {
            delay = Random.Range(0.0f, 0.2f);
            //Instantiate(enemy,new Vector3(Random.Range(-3.5f,3.5f), Random.Range(2f,5f), 0), Quaternion.identity);
            StartCoroutine(SpawnEnemyWithDelay(delay, enemyMarkerPrefab, GetEnemyByProbability(cumulativeProbability)));
        }
    }
    private IEnumerator SpawnEnemyWithDelay(float interval, GameObject enemy, int enemyID)
    {
        yield return new WaitForSeconds(interval);
        if(enemyID <= 3)
        {
            GameObject newEnemy = Instantiate(enemy,new Vector3(Random.Range(redZoneL, redZoneR), Random.Range(redZoneBottom, redZoneTop), 0), Quaternion.identity);
            newEnemy.GetComponent<EnemySpawnPoint>().enemyID = enemyID;
        } else if(enemyID == 4)
        {
            GameObject newEnemy = Instantiate(enemy,new Vector3(Random.Range(blueZoneL, blueZoneR), Random.Range(blueZoneBottom, blueZoneTop), 0), Quaternion.identity);
            newEnemy.GetComponent<EnemySpawnPoint>().enemyID = enemyID;
        } else if(enemyID == 5)
        {
            GameObject newEnemy = Instantiate(enemy,new Vector3(Random.Range(greenZoneL, greenZoneR), Random.Range(greenZoneBottom, greenZoneTop), 0), Quaternion.identity);
            newEnemy.GetComponent<EnemySpawnPoint>().enemyID = enemyID;
        }
        
        
    }

    // private IEnumerator SpawnEnemyWithDelay(float interval, GameObject enemy)
    // {
    //     yield return new WaitForSeconds(interval);
    //     GameObject newEnemy = Instantiate(enemy,new Vector3(Random.Range(-3.5f,3.5f), Random.Range(2f,5f), 0), Quaternion.identity);
    //     StartCoroutine(SpawnEnemy(interval, enemy));
    // }
}
