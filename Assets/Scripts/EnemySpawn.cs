using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemyPrefab1; // Prefab của enemy 1
    public GameObject enemyPrefab2; // Prefab của enemy 2
    public Transform spawnPoint;    // Vị trí spawn
    private float spawnInterval;    // Khoảng thời gian giữa các lần spawn
    private int layerOrderCounter = 0; // Biến đếm để tăng dần Order in Layer
    private int cntEnemy = 0;
    private const int maxLayerOrder = 100; // Giới hạn tối đa cho Order in Layer

    private void Start()
    {
        StartCoroutine(SpawnEnemyCoroutine());
    }

    // Coroutine để spawn enemy sau mỗi khoảng thời gian
    IEnumerator SpawnEnemyCoroutine()
    {
        while (true)
        {
            cntEnemy++;
            int tmp = Random.Range(1, 3);
            SpawnEnemy(tmp);  // Gọi hàm tạo enemy
            spawnInterval = Random.Range(3, 6);
            yield return new WaitForSeconds(spawnInterval);
            if (cntEnemy == 15) yield break;
        }
    }

    private void SpawnEnemy(int cnt)
    {
        GameObject newEnemy;

        // Thêm vị trí spawn ngẫu nhiên để tránh chồng lấn
        Vector3 randomOffset = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0);
        Vector3 spawnPosition = spawnPoint.position + randomOffset;

        if (cnt == 1)
        {
            newEnemy = Instantiate(enemyPrefab1, spawnPosition, Quaternion.identity);
        }
        else
        {
            newEnemy = Instantiate(enemyPrefab2, spawnPosition, Quaternion.identity);
        }

        // Đặt Order in Layer cho enemy mới tạo
        SpriteRenderer spriteRenderer = newEnemy.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = layerOrderCounter;
            layerOrderCounter = (layerOrderCounter + 1) % maxLayerOrder; // Reset khi đạt maxLayerOrder
        }
    }
}
