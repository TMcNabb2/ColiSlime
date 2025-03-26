using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public GameObject enemyPrefab;
        public float spawnTimer;
        public float spawnInterval;
        public int enemiesPerWave;
        public int enemiesSpawnedThisWave;
    }
    public List<Wave> waves;
    public int waveNumber;
    public Transform minPos;
    public Transform maxPos;

    void Update()
    {
        if(PlayerMovement.Instance.gameObject.activeSelf)
        {
            waves[waveNumber].spawnTimer += Time.deltaTime;
            if(waves[waveNumber].spawnTimer >= waves[waveNumber].spawnInterval)
            {
                waves[waveNumber].spawnTimer = 0;
                SpawnEnemy();
            }
            if(waves[waveNumber].enemiesSpawnedThisWave >= waves[waveNumber].enemiesPerWave)
            {
                waves[waveNumber].enemiesSpawnedThisWave = 0;
                if(waves[waveNumber].spawnInterval > 0.3f)
                {
                    waves[waveNumber].spawnInterval *= 0.9f;
                }
                waveNumber++;
            }
            if(waveNumber >= waves.Count)
            {
                waveNumber = 0;
            }
        }
    }
    private void SpawnEnemy()
    {
        Instantiate(waves[waveNumber].enemyPrefab, RandomSpawnPoint(), Quaternion.identity);
        waves[waveNumber].enemiesSpawnedThisWave++;
    }
    private Vector3 RandomSpawnPoint()
    {
        Vector3 spawnPoint;
        if(Random.Range(0f,1f) > 0.5f)
        {
            spawnPoint.x = Random.Range(minPos.position.x, maxPos.position.x);
            if(Random.Range(0f,1f) > 0.5f)
            {
                spawnPoint.z = minPos.position.z;
            } else {
                spawnPoint.z = maxPos.position.z;
            }
        }
        else
        {
            spawnPoint.z = Random.Range(minPos.position.z, maxPos.position.z);
            if(Random.Range(0f,1f) > 0.5f)
            {
                spawnPoint.x = minPos.position.x;
            } else {
                spawnPoint.x = maxPos.position.x;
            }
        }
        spawnPoint.y = 1;
        return spawnPoint;
        //return new Vector3(UnityEngine.Random.Range(-10, 10), 0, UnityEngine.Random.Range(-10, 10));
    }

}
