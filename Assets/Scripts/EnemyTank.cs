using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTank : MonoBehaviour
{
    public int HP = 200; // Đặt HP ban đầu cho EnemyTank
    public static bool isDead = false;

    // Phương thức nhận sát thương
    public void TakeDamage(int damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            Destroy(gameObject); // Hủy đối tượng nếu HP <= 0
            isDead = true;
        }
    }
}