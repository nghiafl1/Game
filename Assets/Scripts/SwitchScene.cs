using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SwitchScene : MonoBehaviour
{
    public static bool checkScene2 = false;
    public static int sc = 0;
    public Image fadeImage; // Image màu đen trong Canvas
    public Image arrowImage; // Mũi tên nhấp nháy
    public float fadeDuration = 2.0f; // Thời gian fade
    private bool isSwitching = false; // Biến để kiểm tra quá trình chuyển cảnh

    private void Start()
    {
        // Đảm bảo fadeImage bắt đầu tối (alpha = 1) và sáng dần lên
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1);

        // Bắt đầu hiệu ứng fade-in ngay khi bắt đầu
        StartCoroutine(Fade(0.0f));

        // Đảm bảo mũi tên không hiển thị lúc đầu
        arrowImage.enabled = false;
    }

    private void Update()
    {
        // Kiểm tra nếu đang ở Scene 7 và không phải trong quá trình chuyển cảnh
        if (sc == 7 && !isSwitching)
        {
            StartCoroutine(ArrowBlinking()); // Bắt đầu hiệu ứng nhấp nháy cho mũi tên
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra nếu sc == 7 và đối tượng va chạm là Player
        if (sc == 7 && collision.CompareTag("Player") && !isSwitching)
        {
            StartCoroutine(FadeAndSwitchScene("Scene2"));
            isSwitching = true; // Đánh dấu rằng quá trình chuyển cảnh đã bắt đầu
        }
    }

    private IEnumerator FadeAndSwitchScene(string newSceneName)
    {
        // Bắt đầu quá trình fade-out (tối dần)
        yield return StartCoroutine(Fade(1.0f));
        Spawm2.cntEnemy = 0;
        // Chuyển cảnh không đồng bộ sau khi fade-out hoàn tất
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(newSceneName);
        asyncLoad.allowSceneActivation = false; // Tạm thời chưa kích hoạt cảnh mới

        // Đợi một chút để đảm bảo cảnh mới sẵn sàng
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f) // Cảnh mới đã load gần xong
            {
                asyncLoad.allowSceneActivation = true; // Kích hoạt cảnh mới
            }
            yield return null;
        }

        checkScene2 = true;
        sc = 100000; // Đặt lại `sc` để không tiếp tục gọi hàm chuyển cảnh trong OnTriggerEnter2D
        isSwitching = false; // Đặt lại `isSwitching` sau khi chuyển cảnh hoàn tất

        // Thực hiện fade-in sau khi cảnh mới xuất hiện
        yield return StartCoroutine(Fade(0.0f));
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeImage.color.a;
        float elapsed = 0.0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeDuration);
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, newAlpha);
            yield return null;
        }

        // Đảm bảo alpha đạt đúng giá trị mục tiêu
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, targetAlpha);
    }

    private IEnumerator ArrowBlinking()
    {
        // Bật mũi tên
        arrowImage.enabled = true;

        // Nhấp nháy mũi tên (alpha thay đổi từ 0 đến 1 rồi ngược lại)
        while (sc == 7)
        {
            // Mũi tên sáng lên
            float alpha = Mathf.PingPong(Time.time * 2f, 1); // Điều chỉnh tốc độ nhấp nháy
            arrowImage.color = new Color(arrowImage.color.r, arrowImage.color.g, arrowImage.color.b, alpha);
            yield return null;
        }

        // Tắt mũi tên khi không còn ở Scene 7
        arrowImage.enabled = false;
    }
}
