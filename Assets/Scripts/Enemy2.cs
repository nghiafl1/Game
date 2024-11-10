using System.Collections;
using UnityEngine;
using TMPro;

public class Enemy2 : MonoBehaviour
{
    public GameObject player;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public GameObject damPopUp; // Prefab cho popup damage

    public float speed = 2f;
    public float stopDistance;
    public float bulletSpeed = 5f;
    public float fireRate = 2f;
    public float maxWaitTime = 1.5f; // Thời gian chờ giữa các lần bắn
    public float hp = 100f; // Máu của enemy

    private Animator animator;
    private bool isMoving = false;
    private float nextFireTime = 0f;
    private float waitTime = 0f;
    private bool isShooting = false; // Kiểm tra xem enemy có đang bắn không
    private bool isDead = false; // Kiểm tra trạng thái chết của enemy

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
        if (isDead) return; // Ngăn mọi hành động nếu enemy đã chết

        MoveTowardsPlayer();

        if (!isMoving)
        {
            waitTime += Time.deltaTime;
            if (waitTime >= maxWaitTime && Time.time >= nextFireTime && !isShooting)
            {
                StartCoroutine(BlinkAndShoot());
            }
        }
        else
        {
            waitTime = 0f;
        }

        if (hp <= 0 && !isDead)
        {
            Die();
            SwitchScene.sc++;
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
        isShooting = true;
        animator.SetTrigger("EnemyShooting");

        yield return new WaitForSeconds(0.9f);

        Shoot();

        nextFireTime = Time.time + maxWaitTime;
        waitTime = 0f;
        isShooting = false;
    }

    void Shoot()
    {
        if (player == null || bulletPrefab == null || firePoint == null || isDead) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        Vector2 direction = (player.transform.position - firePoint.position).normalized;
        rb.velocity = direction * bulletSpeed;
    }

    public void TakeDamEffect(int damage)
    {
        if (isDead || damPopUp == null) return;

        GameObject instance = Instantiate(damPopUp, transform.position
            + new Vector3(Random.Range(-0.3f, 0.3f), 0.5f, 0), Quaternion.identity);

        var textMesh = instance.GetComponentInChildren<TextMeshProUGUI>();
        if (textMesh != null)
            textMesh.text = damage.ToString();

        var popupAnimator = instance.GetComponentInChildren<Animator>();
        if (popupAnimator != null)
        {
            if (damage <= 10) popupAnimator.Play("normal");
            else popupAnimator.Play("critical");
        }

        hp -= damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("bullet") && !isDead)
        {
            int damage = Random.Range(10, 21);
            TakeDamEffect(damage);
            Destroy(collision.gameObject);
        }
    }

    private void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");
        Invoke("DestroyEnemy", 1f); // Điều chỉnh thời gian theo thời lượng animation chết
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
