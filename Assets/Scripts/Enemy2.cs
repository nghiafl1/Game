using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    public GameObject player;
    public float speed = 2f;
    public float minStopDistance = 1f;  // Khoảng cách dừng nhỏ nhất
    public float maxStopDistance = 10f;  // Khoảng cách dừng lớn nhất
    private float stopDistance;  // Khoảng cách dừng ngẫu nhiên

    private Animator animator;
    private bool isMoving = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found! Make sure the Player tag is assigned.");
        }

        animator = GetComponent<Animator>();

        // Gán một khoảng cách ngẫu nhiên từ minStopDistance đến maxStopDistance
        stopDistance = Random.Range(minStopDistance, maxStopDistance);
    }

    void Update()
    {
        MoveTowardsPlayer();
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
}
