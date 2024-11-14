using System.Collections;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    public GameObject player;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public GameObject damPopUp;

    public float speed = 2f;
    public float bulletSpeed = 5f;
    public float fireRate = 2f;
    public float maxWaitTime = 3f;
    public float hp = 100f;

    private Animator animator;
    private bool isMoving = false;
    private float nextFireTime = 0f;
    private float waitTime = 0f;
    private bool isShooting = false;
    private bool isDead = false;

    private Vector2 randomTargetPosition;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found! Make sure the Player tag is assigned.");
        }

        animator = GetComponent<Animator>();
        SetRandomTargetPosition();
    }

    void Update()
    {
        if (isDead) return;
        Flip();
        // Enemy di chuyển đến vị trí randomTargetPosition
        if (Vector2.Distance(transform.position, randomTargetPosition) > 0.1f)
        {
            MoveTowardsTarget();
        }
        else
        {
            // Dừng lại và thực hiện bắn khi đến vị trí
            SetMoving(false);
            waitTime += Time.deltaTime;
            if (waitTime >= maxWaitTime && Time.time >= nextFireTime && !isShooting)
            {
                StartCoroutine(BlinkAndShoot());
                // Sau khi bắn, đặt lại thời gian chờ
                waitTime = 0f;
            }
        }

        if (hp <= 0 && !isDead)
        {
            Die();
            Spawm2.cntEnemy++;
            SwitchScene.sc++;
        }
    }

    void SetRandomTargetPosition()
    {
        float randomX = Random.Range(-9.34f, -1.37f);
        randomTargetPosition = new Vector2(randomX, transform.position.y);
    }

    void MoveTowardsTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, randomTargetPosition, speed * Time.deltaTime);
        SetMoving(true);
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

        yield return new WaitForSeconds(0.6f);

        Shoot();

        nextFireTime = Time.time + maxWaitTime;
        isShooting = false;
    }

    void Shoot()
    {
        if (player == null || bulletPrefab == null || firePoint == null || isDead) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        Vector2 direction = (player.transform.position.x > transform.position.x) ? Vector2.right : Vector2.left;
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
            int damage = Random.Range(25, 26);
            TakeDamEffect(damage);
            Destroy(collision.gameObject);
        }
    }

    private void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");
        Invoke("DestroyEnemy", 1f);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
