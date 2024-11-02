using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    public GameObject player;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float speed = 2f;
    public float stopDistance;
    public float bulletSpeed = 5f;
    public float fireRate = 2f;

    private Animator animator;
    private bool isMoving = false;
    private float nextFireTime = 0f;
    private float waitTime = 0f;
    public float maxWaitTime = 3f; // Thời gian chờ giữa các lần bắn
    private bool isShooting = false; // Kiểm tra xem enemy có đang bắn không

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found! Make sure the Player tag is assigned.");
        }

        animator = GetComponent<Animator>();
        stopDistance = Random.Range(5f, 10f);
    }

    void Update()
    {
        MoveTowardsPlayer();

        // Nếu enemy không di chuyển
        if (!isMoving)
        {
            waitTime += Time.deltaTime; // Tăng waitTime

            // Nếu không di chuyển trong 3 giây và thời gian bắn đã đến
            if (waitTime >= maxWaitTime && Time.time >= nextFireTime && !isShooting)
            {
                StartCoroutine(BlinkAndShoot()); // Bắt đầu coroutine bắn
            }
        }
        else
        {
            // Reset waitTime nếu enemy di chuyển
            waitTime = 0f;
        }
    }

    void MoveTowardsPlayer()
    {
        if (player == null) return;
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance > stopDistance)
        {
            Vector2 target = new Vector2(player.transform.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
            SetMoving(true);
        }
        else
        {
            SetMoving(false);
        }

        Flip();
    }

    void SetMoving(bool moving)
    {
        isMoving = moving;
        animator.SetBool("isMoving", isMoving);
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        if ((transform.position.x > player.transform.position.x && scale.x > 0) ||
            (transform.position.x < player.transform.position.x && scale.x < 0))
        {
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    private IEnumerator BlinkAndShoot()
    {
        isShooting = true; // Đánh dấu là đang bắn
        animator.SetTrigger("EnemyShooting"); // Kích hoạt animation bắn

        // Đợi cho đến khi animation bắn hoàn tất
        yield return new WaitForSeconds(0.6f); // Thay đổi giá trị này nếu thời gian animation bắn khác

        Shoot(); // Bắn viên đạn

        nextFireTime = Time.time + maxWaitTime; // Đặt thời gian bắn tiếp theo sau 3 giây
        waitTime = 0f; // Reset thời gian chờ sau khi bắn
        isShooting = false; // Đánh dấu là không còn bắn
    }

    void Shoot()
    {
        if (player == null) return;
        if (bulletPrefab != null && firePoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            Vector2 direction = (player.transform.position - firePoint.position).normalized;
            rb.velocity = direction * bulletSpeed;
        }
    }
}
