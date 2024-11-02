using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemyPrefab;  // Prefab của enemy
    public Transform spawnPoint;    // Vị trí spawn
    public float spawnInterval = 2f; // Khoảng thời gian giữa các lần spawn

    private void Start()
    {
        StartCoroutine(SpawnEnemyCoroutine());
    }

    // Coroutine để spawn enemy sau mỗi khoảng thời gian
    IEnumerator SpawnEnemyCoroutine()
    {
        while (true)
        {
            SpawnEnemy();  // Gọi hàm tạo enemy
            yield return new WaitForSeconds(spawnInterval); // Chờ 2 giây
        }
    }

    private void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
    }
}
