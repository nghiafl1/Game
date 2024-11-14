using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float maxDistance = 10f; // Khoảng cách tối đa viên đạn có thể bay
    private Vector3 startPosition;  // Lưu vị trí ban đầu của viên đạn

    void Start()
    {
        // Lưu vị trí ban đầu của viên đạn khi nó được tạo ra
        startPosition = transform.position;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra va chạm với EnemyTank
        EnemyTank enemyTank = other.GetComponent<EnemyTank>();
        if (enemyTank != null)
        {
            int damage = Random.Range(15, 21); // Gây sát thương ngẫu nhiên từ 15 đến 20
            enemyTank.TakeDamage(damage);
            Destroy(gameObject); // Hủy viên đạn sau khi gây sát thương
        }
    }
    void Update()
    {
        // Kiểm tra nếu viên đạn đã đi xa hơn khoảng cách tối đa
        if (Vector3.Distance(startPosition, transform.position) >= maxDistance)
        {
            Destroy(gameObject); // Xóa viên đạn nếu vượt quá khoảng cách
        }
    }
}
