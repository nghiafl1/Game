using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemyPrefab1;  // Prefab của enemy 1
    public GameObject enemyPrefab2;  // Prefab của enemy 2
    public Transform spawnPoint;    // Vị trí spawn
    float spawnInterval; // Khoảng thời gian giữa các lần spawn

    private void Start()
    {
        StartCoroutine(SpawnEnemyCoroutine());
    }

    // Coroutine để spawn enemy sau mỗi khoảng thời gian
    IEnumerator SpawnEnemyCoroutine()
    {
        while (true)
        {
            int tmp = Random.Range(1, 3);
            SpawnEnemy(tmp);  // Gọi hàm tạo enemy
            spawnInterval = Random.Range(3, 5);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnEnemy(int cnt)
    {
        if(cnt == 1)
        {
            Instantiate(enemyPrefab1, spawnPoint.position, Quaternion.identity);
        }
        else
        {
            Instantiate(enemyPrefab2, spawnPoint.position, Quaternion.identity);
        }
    }
}
