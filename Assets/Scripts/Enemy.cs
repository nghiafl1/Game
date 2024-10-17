using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    public GameObject player; 
    public float speed = 2f;  
    public float stopDistance = 1f;  

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
    }

    void Update()
    {
        MoveTowardsPlayer();
    }

    void MoveTowardsPlayer()
    {
    
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
