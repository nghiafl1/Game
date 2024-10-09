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
    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Trai_phai = Input.GetAxis("Horizontal");
        Rb.velocity = new Vector2(Trai_phai * Speed, Rb.velocity.y);
        Flip();
        animator.SetFloat("move", Mathf.Abs(Trai_phai));
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
}
