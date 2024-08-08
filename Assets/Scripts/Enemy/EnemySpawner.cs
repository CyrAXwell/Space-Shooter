using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemySpawnPoint enemyMarkerPrefab;
    [SerializeField] private Enemy[] enemysPrefabs;
    [SerializeField] private Boss bossPrefab;
    [SerializeField] private float spawnInterval;
    [SerializeField] private float spawnEnemyAmount;
    [SerializeField] private Transform redZone;
    [SerializeField] private Transform blueZone;
    [SerializeField] private Transform greenZone;

    private Player _player;
    private WaveManager _waveManager;
    private int[] _enemySpawnProbability = {0, 0, 0, 0, 0, 0};
    private int[] _cumulativeProbability = {0, 0, 0, 0, 0, 0};
    private float _timer;
    private List<Enemy> _enemiesOnScene = new List<Enemy>();
    private List<GameObject> _spawnPointsOnScene = new List<GameObject>();
    private float _redZoneL;
    private float _redZoneR;
    private float _redZoneTop;
    private float _redZoneBottom;
    private float _blueZoneL;
    private float _blueZoneR;
    private float _blueZoneTop;
    private float _blueZoneBottom;
    private float _greenZoneL;
    private float _greenZoneR;
    private float _greenZoneTop;
    private float _greenZoneBottom;

    public void Initialize(Player player, WaveManager waveManager)
    {
        _player = player;
        _waveManager = waveManager;
        waveManager.OnWaveComplete += OnWaveComplete;
        _timer = spawnInterval - 2f;

        _redZoneL = redZone.position.x - redZone.localScale.x / 2;
        _redZoneR = redZone.position.x + redZone.localScale.x / 2;
        _redZoneTop = redZone.position.y + redZone.localScale.y / 2;
        _redZoneBottom = redZone.position.y - redZone.localScale.y / 2;

        _blueZoneL = blueZone.position.x - blueZone.localScale.x / 2;
        _blueZoneR = blueZone.position.x + blueZone.localScale.x / 2;
        _blueZoneTop = blueZone.position.y + blueZone.localScale.y / 2;
        _blueZoneBottom = blueZone.position.y - blueZone.localScale.y / 2;

        _greenZoneL = greenZone.position.x - greenZone.localScale.x / 2;
        _greenZoneR = greenZone.position.x + greenZone.localScale.x / 2;
        _greenZoneTop = greenZone.position.y + greenZone.localScale.y / 2;
        _greenZoneBottom = greenZone.position.y - greenZone.localScale.y / 2;

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
            case 1: _enemySpawnProbability = new int[] {8000, 2000, 0, 0, 0, 0}; break;          //{8000, 2000, 0, 0, 0, 0}                  
            case 2: _enemySpawnProbability = new int[] {8000, 2000, 0, 0, 0, 0}; break;          //{7000, 3000, 0, 0, 0, 0} 
            case 3: _enemySpawnProbability = new int[] {6200, 2000, 1800, 0, 0, 0}; break;       //{6000, 2500, 1500, 0, 0, 0}  
            case 4: _enemySpawnProbability = new int[] {6200, 2000, 1800, 0, 0, 0}; break;       //{5500, 2500, 2000, 0, 0, 0}  
            case 5: _enemySpawnProbability = new int[] {4700, 2000, 1800, 0, 1500, 0}; break;    //{4000, 2500, 2000, 0, 1500, 0}
            case 6: _enemySpawnProbability = new int[] {4700, 2000, 1800, 0, 1500, 0}; break;    //{4000, 2500, 2000, 0, 1500, 0}   
            case 7: _enemySpawnProbability = new int[] {4700, 2000, 1800, 0, 1500, 0}; break;    //{4000, 2500, 2000, 0, 1500, 0}     
            case 8: _enemySpawnProbability = new int[] {3600, 2000, 1800, 0, 1500, 1100}; break; //{3000, 2500, 2000, 0, 1500, 1000}
            case 9: _enemySpawnProbability = new int[] {3600, 2000, 1800, 0, 1500, 1100}; break; //{3000, 2400, 2000, 0, 1500, 1100} 
            case 10: _enemySpawnProbability = new int[] {3600, 2000, 1800, 0, 1500, 1100}; break;//{3000, 2400, 2000, 0, 1500, 1100}  
            case >= 11: _enemySpawnProbability = new int[] {3900, 2000, 1800, 700, 1100, 500}; break; //{2500, 2000, 1800, 1100, 1500, 1100}         
        }
        if(wave == 5)
            spawnEnemyAmount ++;    

        GetProbability(_enemySpawnProbability);
        _timer = spawnInterval - 2f;
    }

    private int GetEnemyByProbability(int[] probability)
    {
        int randomNumber = Random.Range(0, 10001);
        for (int i = 0; i < probability.Length; i++)
        {
            if (randomNumber <= _cumulativeProbability[i])
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
            _cumulativeProbability[i] = probabilitySum;
        }
    }

    private void SpawnEnemy(float numOfEnemy)
    {
        for(var i = 1; i <= numOfEnemy; i++)
        {
            float delay = Random.Range(0.0f, 0.2f);
            StartCoroutine(SpawnEnemyWithDelay(delay, enemyMarkerPrefab, GetEnemyByProbability(_cumulativeProbability)));
        }
    }

    private void OnWaveComplete()
    {
        StopAllCoroutines();
        foreach (GameObject spawnPoint in _spawnPointsOnScene)
            Destroy(spawnPoint);
    }

    private IEnumerator SpawnEnemyWithDelay(float interval, EnemySpawnPoint marker, int enemyID)
    {
        float minXPos = 0;
        float maxXPos = 0;
        float minYPos = 0;
        float maxYPos = 0;

        if(enemyID <= 3)
        {
            minXPos = _redZoneL;
            maxXPos = _redZoneR;
            minYPos = _redZoneBottom;
            maxYPos = _redZoneTop;
        } 
        else if(enemyID == 4)
        {
            minXPos = _blueZoneL;
            maxXPos = _blueZoneR;
            minYPos = _blueZoneBottom;
            maxYPos = _blueZoneTop;
        } 
        else if(enemyID == 5)
        {
            minXPos = _greenZoneL;
            maxXPos = _greenZoneR;
            minYPos = _greenZoneBottom;
            maxYPos = _greenZoneTop;
        } 

        Vector3 spawnPos = new Vector3(Random.Range(minXPos, maxXPos), Random.Range(minYPos, maxYPos), 0);

        yield return new WaitForSeconds(interval);
        GameObject enemySpawnPoint = Instantiate(marker.gameObject, spawnPos, Quaternion.identity);
        _spawnPointsOnScene.Add(enemySpawnPoint);
        
        yield return new WaitForSeconds(1.5f);

        Destroy(enemySpawnPoint);
        Enemy newEnemy = Instantiate(enemysPrefabs[enemyID].gameObject, spawnPos, Quaternion.identity).GetComponent<Enemy>();
        newEnemy.Initialize(_player, _waveManager.GetWave());
        //_enemiesOnScene.Add(newEnemy);
        //CreateEnemySpawnPoint(marker, minXPos, maxXPos, minYPos, maxYPos, enemyID);
    }

    private void CreateEnemySpawnPoint(EnemySpawnPoint marker, float minXPos, float maxXPos, float minYPos, float maxYPos, int enemyID)
    {
        GameObject newEnemySpawnPoint = Instantiate(marker.gameObject, new Vector3(Random.Range(minXPos, maxXPos), Random.Range(minYPos, maxYPos), 0), Quaternion.identity);
        newEnemySpawnPoint.GetComponent<EnemySpawnPoint>().Initialize(enemysPrefabs[enemyID]);
    }
}
