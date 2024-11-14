using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Speed;
    private float Trai_phai;
    private float Len_xuong; // Biến để lưu giá trị trục dọc
    private bool IsFacingRight = true;
    private Rigidbody2D Rb;
    private Animator animator;
    public GameObject Bullet;
    public GameObject Bullet2;
    public Transform FirePos;
    public float SpeedFire = 1f;
    public float BulletForce;
    private float speedFire;
    public int DamageEnemy;
    private Vector3 vtplayer;

    void Start()
    {
        vtplayer = new Vector3(-35.44529f, -0.2434353f, 0); // Vị trí mong muốn khi chuyển sang Scene2
        Rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Kiểm tra nếu đang ở Scene2, đặt player vào vị trí vtplayer
        if (SwitchScene.checkScene2)
        {
            SetPlayerPosition();
            SwitchScene.checkScene2 = false;
        }
    }

    void Update()
    {
        // Kiểm tra nếu checkScene2 được bật để đặt lại vị trí
        if (SwitchScene.checkScene2)
        {
            SetPlayerPosition();
            SwitchScene.checkScene2 = false;
        }

        Trai_phai = Input.GetAxisRaw("Horizontal");
        Len_xuong = Input.GetAxisRaw("Vertical"); // Lấy giá trị trục dọc

        // Di chuyển theo cả hai trục
        Rb.velocity = new Vector2(Trai_phai * Speed, Len_xuong * Speed);

        Flip();
        animator.SetFloat("move", Mathf.Abs(Trai_phai) + Mathf.Abs(Len_xuong)); // Cập nhật animation khi di chuyển

        speedFire -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && speedFire < 0)
        {
            FireBullet();
        }
    }

    void SetPlayerPosition()
    {
        transform.position = vtplayer;
        Flip2();
    }

    void Flip()
    {
        if (IsFacingRight && Trai_phai < 0 || !IsFacingRight && Trai_phai > 0)
        {
            IsFacingRight = !IsFacingRight;
            Vector3 Kich_thuoc = transform.localScale;
            Kich_thuoc.x *= -1;
            transform.localScale = Kich_thuoc;
        }
    }

    void Flip2()
    {
        if (IsFacingRight == false)
        {
            IsFacingRight = !IsFacingRight;
            Vector3 Kich_thuoc = transform.localScale;
            Kich_thuoc.x *= -1;
            transform.localScale = Kich_thuoc;
        }
    }

    void FireBullet()
    {
        speedFire = SpeedFire;
        GameObject bullet;
        Rigidbody2D rb;

        if (IsFacingRight)
        {
            bullet = Instantiate(Bullet, FirePos.position, Quaternion.identity);
            rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = transform.right * BulletForce;
        }
        else
        {
            bullet = Instantiate(Bullet2, FirePos.position, Quaternion.identity);
            rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = -transform.right * BulletForce;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BulletEnemy"))
        {
            DamageEnemy = Random.Range(2, 4);
        }
        else if (collision.CompareTag("BulletEnemy2"))
        {
            DamageEnemy = 1;
        }
        else if (collision.CompareTag("BulletEnemyTank"))
        {
            DamageEnemy = 15;
        }
        else if (collision.CompareTag("ChasingBullet"))
        {
            DamageEnemy = 7;
        }
    }
}