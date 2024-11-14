using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemyPrefab1;
    public GameObject enemyPrefab2;
    public Transform spawnPoint;
    private float spawnInterval;
    private int layerOrderCounter = 5;
    private int cntEnemy = 0;
    private const int maxLayerOrder = 100;
    public float minDistanceBetweenEnemies = 2f; // Khoảng cách tối thiểu giữa các enemy

    private List<GameObject> spawnedEnemies = new List<GameObject>(); // Danh sách các enemy đã spawn

    private void Start()
    {
        StartCoroutine(SpawnEnemyCoroutine());
    }

    IEnumerator SpawnEnemyCoroutine()
    {
        while (true)
        {
            cntEnemy++;
            int tmp = Random.Range(1, 3);
            SpawnEnemy(tmp);
            spawnInterval = Random.Range(3, 6);
            yield return new WaitForSeconds(spawnInterval);
            if (cntEnemy == 7) yield break;
        }
    }

    private void SpawnEnemy(int cnt)
    {
        GameObject newEnemy;
        Vector3 spawnPosition;
        bool validPosition = false;

        // Lặp lại cho đến khi tìm được vị trí hợp lệ
        do
        {
            float randomX = 0.62f;
            float randomY = Random.Range(-3f, 0.16f);
            spawnPosition = new Vector3(randomX, randomY, 0);

            // Kiểm tra khoảng cách với các enemy đã spawn
            validPosition = true;
            foreach (var enemy in spawnedEnemies)
            {
                // Kiểm tra nếu enemy đã bị phá hủy
                if (enemy == null) continue;

                if (Vector3.Distance(spawnPosition, enemy.transform.position) < minDistanceBetweenEnemies)
                {
                    validPosition = false;
                    break;
                }
            }
        } while (!validPosition);

        // Tạo enemy tại vị trí hợp lệ
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
            layerOrderCounter = (layerOrderCounter + 1) % maxLayerOrder;
        }

        // Thêm enemy vào danh sách để kiểm tra vị trí cho các lần spawn sau
        spawnedEnemies.Add(newEnemy);
    }

}