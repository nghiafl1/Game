using UnityEngine;

public class ChasingBullet : MonoBehaviour
{
    public float speed = 3f; // Tốc độ di chuyển chậm hơn để Player dễ né
    public Transform target;
    public float lifeTime = 4.5f; // Thời gian sống của đạn
    public float rotationSpeed = 100f; // Tốc độ xoay, giúp đạn không đổi hướng quá nhanh

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        if (target == null) return;

        // Tìm hướng tới mục tiêu
        Vector3 direction = (target.position - transform.position).normalized;

        // Xoay từ từ để đạn không đuổi kịp nếu Player di chuyển nhanh
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Di chuyển đạn theo hướng hiện tại
        transform.position += transform.right * speed * Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
