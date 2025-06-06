using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemyManager : MonoBehaviour
{
    [Header("怪物设置")]
    public GameObject[] enemyPrefabs; // 两种怪物预制体
    public Transform[] spawnPoints;   // 出生点数组
    public float spawnInterval = 3f;  // 出怪间隔
    public int maxEnemies = 10;       // 场景最大怪物数量

    [Header("波次设置")]
    public int currentWave = 1;
    public int enemiesPerWave = 5;
    public float waveInterval = 10f;

    private int enemiesSpawnedThisWave = 0;
    private int activeEnemies = 0;

    protected void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(WaveSpawner());
        }
    }

    IEnumerator WaveSpawner()
    {
        while (true)
        {
            yield return new WaitForSeconds(waveInterval);
            StartCoroutine(SpawnWave());
        }
    }

    IEnumerator SpawnWave()
    {
        enemiesSpawnedThisWave = 0;
        Debug.Log($"开始第 {currentWave} 波攻击!");

        while (enemiesSpawnedThisWave < enemiesPerWave && activeEnemies < maxEnemies)
        {
            SpawnEnemy();
            enemiesSpawnedThisWave++;
            activeEnemies++;
            yield return new WaitForSeconds(spawnInterval);
        }

        currentWave++;
        enemiesPerWave = Mathf.RoundToInt(enemiesPerWave * 1.5f); // 每波增加50%怪物数量
    }

    void SpawnEnemy()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        // 随机选择怪物类型和出生点
        int enemyIndex = Random.Range(0, enemyPrefabs.Length);
        int spawnIndex = Random.Range(0, spawnPoints.Length);

        // 网络实例化怪物
        PhotonNetwork.Instantiate(
            $"Enemy/{enemyPrefabs[enemyIndex].name}",
            spawnPoints[spawnIndex].position,
            spawnPoints[spawnIndex].rotation
        );
    }

    // 怪物死亡时调用
    [PunRPC]
    public void EnemyDied()
    {
        activeEnemies--;
    }
}
