using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    public float maxDistance = 12f; // Khoảng cách tối đa viên đạn có thể bay
    private Vector3 startPosition;  // Lưu vị trí ban đầu của viên đạn

    void Start()
    {
        // Lưu vị trí ban đầu của viên đạn khi nó được tạo ra
        startPosition = transform.position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
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
