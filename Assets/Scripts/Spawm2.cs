using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Spawm2 : MonoBehaviour
{
    public GameObject enemyPrefab1;
    public GameObject enemyPrefab2;
    public GameObject enemyTankPrefab;
    public GameObject bulletPrefab;         // Đạn thông thường
    public GameObject chasingBulletPrefab;   // Đạn truy đuổi mới
    public Transform spawnPoint;
    private float spawnInterval;
    private int layerOrderCounter = 5;
    public static int cntEnemy = 0;
    private const int maxLayerOrder = 100;
    public float minDistanceBetweenEnemies = 2f;
    private CinemachineVirtualCamera cinemachineCamera;

    private List<GameObject> spawnedEnemies = new List<GameObject>();

    private void Start()
    {
        cinemachineCamera = FindObjectOfType<CinemachineVirtualCamera>(); // Lấy Cinemachine từ Scene1
        if (cinemachineCamera != null)
        {
            cinemachineCamera.Follow = GameObject.FindWithTag("Player").transform;
        }
        StartCoroutine(SpawnEnemyCoroutine());
    }

    IEnumerator SpawnEnemyCoroutine()
    {
        while (cntEnemy < 7)
        {
            int tmp = Random.Range(1, 3);
            SpawnEnemy(tmp);
            spawnInterval = Random.Range(3, 6);
            yield return new WaitForSeconds(spawnInterval);
        }

        yield return new WaitForSeconds(3);

        Vector3 tankSpawnPosition = new Vector3(5.46f, -2.6f, 0.04250026f);
        GameObject enemyTank = Instantiate(enemyTankPrefab, tankSpawnPosition, Quaternion.identity);

        StartCoroutine(MoveAndShootEnemyTank(enemyTank, -4.07f));
    }

    private void SpawnEnemy(int cnt)
    {
        GameObject newEnemy;
        Vector3 spawnPosition;
        bool validPosition = false;

        do
        {
            float randomX = 0.62f;
            float randomY = Random.Range(-3f, 0.16f);
            spawnPosition = new Vector3(randomX, randomY, 0);

            validPosition = true;
            foreach (var enemy in spawnedEnemies)
            {
                if (enemy == null) continue;
                if (Vector3.Distance(spawnPosition, enemy.transform.position) < minDistanceBetweenEnemies)
                {
                    validPosition = false;
                    break;
                }
            }
        } while (!validPosition);

        if (cnt == 1)
        {
            newEnemy = Instantiate(enemyPrefab1, spawnPosition, Quaternion.identity);
        }
        else
        {
            newEnemy = Instantiate(enemyPrefab2, spawnPosition, Quaternion.identity);
        }

        SpriteRenderer spriteRenderer = newEnemy.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = layerOrderCounter;
            layerOrderCounter = (layerOrderCounter + 1) % maxLayerOrder;
        }

        spawnedEnemies.Add(newEnemy);
    }

    IEnumerator MoveAndShootEnemyTank(GameObject enemyTank, float targetX)
    {
        float speed = 2f;
        Animator animator = enemyTank.GetComponent<Animator>();

        Vector3 targetPosition = new Vector3(targetX, enemyTank.transform.position.y, 0f);
        animator.SetBool("isMoving", true);

        while (Vector3.Distance(enemyTank.transform.position, targetPosition) > 0.1f)
        {
            enemyTank.transform.position = Vector3.MoveTowards(enemyTank.transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }
        if (cinemachineCamera != null)
        {
            cinemachineCamera.Follow = null;
        }
        while (true)
        {
            float randomY = Random.Range(-2.97f, 0.61f);
            targetPosition = new Vector3(targetX, randomY, 0f);

            animator.SetBool("isMoving", true);
            while (Vector3.Distance(enemyTank.transform.position, targetPosition) > 0.1f)
            {
                enemyTank.transform.position = Vector3.MoveTowards(enemyTank.transform.position, targetPosition, speed * Time.deltaTime);
                yield return null;
            }

            animator.SetBool("isMoving", false);

            // Gọi hiệu ứng bắn rồi tạo đạn
            yield return StartCoroutine(Shoot(enemyTank));

            yield return new WaitForSeconds(2f); // Đợi 2 giây trước khi EnemyTank di chuyển lần nữa
        }
    }


    private IEnumerator Shoot(GameObject enemyTank)
    {
        Animator animator = enemyTank.GetComponent<Animator>();

        if (animator != null)
        {
            animator.SetTrigger("shoot");
        }

        yield return new WaitForSeconds(0.6f);

        // Vị trí bắn đạn thường
        Transform bulletSpawnPoint = enemyTank.transform.Find("BulletSpawnPoint");
        Vector3 bulletSpawnPosition = bulletSpawnPoint != null
            ? bulletSpawnPoint.position
            : enemyTank.transform.position + Vector3.left * 0.5f;

        // Bắn viên đạn thường
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPosition, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.left * 5f;
        }

        // Tìm vị trí bắn đạn truy đuổi trên enemyTank
        Transform chasingBulletSpawnPoint = enemyTank.transform.Find("ChasingBulletSpawnPoint");
        Vector3 chasingBulletSpawnPosition = chasingBulletSpawnPoint != null
            ? chasingBulletSpawnPoint.position
            : enemyTank.transform.position + Vector3.left * 0.5f;

        // Bắn viên đạn truy đuổi
        GameObject chasingBullet = Instantiate(chasingBulletPrefab, chasingBulletSpawnPosition, Quaternion.identity);
        ChasingBullet chasingScript = chasingBullet.GetComponent<ChasingBullet>();
        chasingScript.target = GameObject.FindWithTag("Player").transform;
        chasingScript.speed = 3f; // Tốc độ truy đuổi chậm hơn để Player có thể né
        chasingScript.rotationSpeed = 100f; // Tốc độ xoay giúp đạn không đổi hướng quá nhanh
    }
}