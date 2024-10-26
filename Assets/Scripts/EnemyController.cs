using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject damPopUp; // Prefab cho popup damage
    private float hp = 100;     // Máu của kẻ địch

    // Hàm để tạo hiệu ứng khi nhận damage
    public void TakeDamEffect(int damage)
    {
        if (damPopUp != null)
        {
            // Tạo popup tại vị trí kẻ địch với chút ngẫu nhiên về tọa độ
            GameObject instance = Instantiate(damPopUp, transform.position
                + new Vector3(Random.Range(-0.3f, 0.3f), 0.5f, 0), Quaternion.identity);

            // Gán text hiển thị giá trị damage
            var textMesh = instance.GetComponentInChildren<TextMeshProUGUI>();
            if (textMesh != null)
                textMesh.text = damage.ToString();

            // Lấy Animator và phát animation dựa trên damage
            var animator = instance.GetComponentInChildren<Animator>();
            if (animator != null)
            {
                if (damage <= 10) animator.Play("normal");
                else animator.Play("critical");
            }
        }
    }

    // Xử lý va chạm với viên đạn
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("bullet"))
        {
            // Tạo giá trị damage ngẫu nhiên
            int damage = Random.Range(1, 20);

            // Trừ máu kẻ địch
            hp -= damage;

            // Hiển thị hiệu ứng damage
            TakeDamEffect(damage);

            // Hủy viên đạn sau va chạm
            Destroy(collision.gameObject);
        }
    }

    // Kiểm tra trạng thái kẻ địch trong mỗi frame
    private void Update()
    {
        if (hp <= 0)
        {
            Destroy(gameObject); // Hủy đối tượng nếu HP <= 0
        }
    }
}