using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Speed;
    private float Trai_phai;
    private bool IsFacingRight = true;
    private Rigidbody2D Rb;
    private Animator animator;
    public GameObject Bullet;
    public GameObject Bullet2;
    public Transform FirePos;
    public float SpeedFire = 1f;
    public float BulletForce;
    private float speedFire;
    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Trai_phai = Input.GetAxisRaw("Horizontal");
        //if ((transform.position.x < -8.73 && Trai_phai < 0) || (transform.position.x > 8.73 && Trai_phai > 0)) return;
        Rb.velocity = new Vector2(Trai_phai * Speed, Rb.velocity.y);
        Flip();
        animator.SetFloat("move", Mathf.Abs(Trai_phai));
        speedFire -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && speedFire < 0) 
        {
            FireBullet();
        }
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
    void FireBullet()
    {
        speedFire = SpeedFire;
        if (IsFacingRight == true)
        {           
            GameObject bullet = Instantiate(Bullet, FirePos.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(transform.right * BulletForce, ForceMode2D.Impulse);
        }
        else
        { 
            GameObject bullet = Instantiate(Bullet2, FirePos.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(-transform.right * BulletForce, ForceMode2D.Impulse);
        }
    }
}
