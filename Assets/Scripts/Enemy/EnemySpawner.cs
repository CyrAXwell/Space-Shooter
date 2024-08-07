using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyMarkerPrefab;
    [SerializeField] private Enemy[] enemysPrefabs;
    [SerializeField] private Boss bossPrefab;
    [SerializeField] private float spawnInterval;
    [SerializeField] private float spawnEnemyAmount;
    [SerializeField] private Transform redZone;
    [SerializeField] private Transform blueZone;
    [SerializeField] private Transform greenZone;

    private float redZoneL;
    private float redZoneR;
    private float redZoneTop;
    private float redZoneBottom;
    private float blueZoneL;
    private float blueZoneR;
    private float blueZoneTop;
    private float blueZoneBottom;
    private float greenZoneL;
    private float greenZoneR;
    private float greenZoneTop;
    private float greenZoneBottom;

    private int[] enemySpawnProbability = {0, 0, 0, 0, 0, 0};
    private int[] cumulativeProbability = {0, 0, 0, 0, 0, 0};

    private float _timer;

    void Start()
    {
        _timer = spawnInterval - 2f;

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

        UpdateProbability(1);
    }

    private void Update()
    {
        _timer -= Time.deltaTime;
        if(_timer <= 0)
        {
            _timer = spawnInterval;
            SpawnEnemy(spawnEnemyAmount);
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
            spawnEnemyAmount ++;    

        GetProbability(enemySpawnProbability);
        _timer = spawnInterval - 2f;
    }

    private int GetEnemyByProbability(int[] probability)
    {
        int randomNumber = Random.Range(0, 10001);
        for (int i = 0; i < probability.Length; i++)
        {
            if (randomNumber <= cumulativeProbability[i])
                return i;
        }
        return -1;
    }

    private void GetProbability(int[] enemyProbability)
    {
        int probabilitySum = 0;
        for (int i = 0; i < enemyProbability.Length; i++)
        {
            probabilitySum += enemyProbability[i];
            cumulativeProbability[i] = probabilitySum;
        }
    }

    private void SpawnEnemy(float numOfEnemy)
    {
        for(var i = 1; i <= numOfEnemy; i++)
        {
            float delay = Random.Range(0.0f, 0.2f);
            StartCoroutine(SpawnEnemyWithDelay(delay, enemyMarkerPrefab, GetEnemyByProbability(cumulativeProbability)));
        }
    }

    private IEnumerator SpawnEnemyWithDelay(float interval, GameObject marker, int enemyID)
    {
        float minXPos = 0;
        float maxXPos = 0;
        float minYPos = 0;
        float maxYPos = 0;

        yield return new WaitForSeconds(interval);

        if(enemyID <= 3)
        {
            minXPos = redZoneL;
            maxXPos = redZoneR;
            minYPos = redZoneBottom;
            maxYPos = redZoneTop;
        } 
        else if(enemyID == 4)
        {
            minXPos = blueZoneL;
            maxXPos = blueZoneR;
            minYPos = blueZoneBottom;
            maxYPos = blueZoneTop;
        } 
        else if(enemyID == 5)
        {
            minXPos = greenZoneL;
            maxXPos = greenZoneR;
            minYPos = greenZoneBottom;
            maxYPos = greenZoneTop;
        } 

        Enemy newEnemy = Instantiate(enemysPrefabs[enemyID], new Vector3(Random.Range(minXPos, maxXPos), Random.Range(minYPos, maxYPos), 0), Quaternion.identity);

        //CreateEnemySpawnPoint(marker, minXPos, maxXPos, minYPos, maxYPos, enemyID);
    }

    private void CreateEnemySpawnPoint(GameObject marker, float minXPos, float maxXPos, float minYPos, float maxYPos, int enemyID)
    {
        GameObject newEnemySpawnPoint = Instantiate(marker, new Vector3(Random.Range(minXPos, maxXPos), Random.Range(minYPos, maxYPos), 0), Quaternion.identity);
        newEnemySpawnPoint.GetComponent<EnemySpawnPoint>().Initialize(enemysPrefabs[enemyID]);
    }
}
