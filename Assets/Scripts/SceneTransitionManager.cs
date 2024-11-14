using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionManager : MonoBehaviour
{
    public Image flashImage; // Tham chiếu tới Image để tạo hiệu ứng flash trắng
    public float flashSpeed = 1.0f; // Tốc độ chuyển Alpha cho hiệu ứng flash
    public string nextSceneName = "Scene3"; // Tên Scene tiếp theo

    // Phương thức này sẽ được gọi khi EnemyTank chết
    private void Update()
    {
        if (EnemyTank.isDead) 
        {
            StartCoroutine(FlashAndTransition());
            EnemyTank.isDead = false;
        }
    }

    private IEnumerator FlashAndTransition()
    {
        yield return new WaitForSeconds(1);
        // Tăng dần Alpha của flashImage để tạo hiệu ứng flash trắng
        while (flashImage.color.a < 1)
        {
            Color tempColor = flashImage.color;
            tempColor.a += Time.deltaTime * flashSpeed;
            flashImage.color = tempColor;
            yield return null;
        }

        // Đợi 1 giây với màn hình trắng để tạo hiệu ứng
        yield return new WaitForSeconds(1);

        // Xóa các đối tượng Player và Canvas trước khi chuyển sang Scene3
        DestroyPlayerAndCanvas();

        // Chuyển sang Scene tiếp theo
        SceneManager.LoadScene(nextSceneName);
    }

    // Hàm này sẽ tìm và hủy Player và Canvas thanh máu
    private void DestroyPlayerAndCanvas()
    {
        // Tìm đối tượng Player và Canvas trong Scene2
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Destroy(player); // Hủy Player
        }

        // Tìm Canvas có tag hoặc tên phù hợp (thanh máu)
        GameObject canvas = GameObject.Find("HealthCanvas"); // Nếu Canvas thanh máu có tên là "HealthCanvas"
        if (canvas != null)
        {
            Destroy(canvas); // Hủy Canvas thanh máu
        }
    }
}
