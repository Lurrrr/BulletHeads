using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;




public class PropManager : MonoBehaviourPun
{
    [Header("道具设置")]
    public GameObject[] PropPrefab; // 两种道具预制体
    public Transform[] spawnPoints;   // 出生点数组
    public float spawnInterval = 10f;  // 出道具间隔
    public int maxProps = 2;       // 场景最大道具数量

    [Header("波次设置")]
    public int currentWave = 1;
    public int propsPerWave = 5;
    public float waveInterval = 10f;

    private int propSpawnedThisWave = 0;
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
        propSpawnedThisWave = 0;
        Debug.Log($"开始第 {currentWave} 波道具生成!");

        while (propSpawnedThisWave < propsPerWave && activeEnemies < maxProps)
        {
            SpawnEnemy();
            propSpawnedThisWave++;
            activeEnemies++;
            yield return new WaitForSeconds(spawnInterval);
        }

        currentWave++;
        propsPerWave = Mathf.RoundToInt(propsPerWave * 1.2f); // 每波增加50%道具数量
    }

    void SpawnEnemy()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        // 随机选择怪物类型和出生点
        int enemyIndex = Random.Range(0, PropPrefab.Length);
        int spawnIndex = Random.Range(0, spawnPoints.Length);

        // 网络实例化怪物
        PhotonNetwork.Instantiate(
            $"Props/{PropPrefab[enemyIndex].name}",
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