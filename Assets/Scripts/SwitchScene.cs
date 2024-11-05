using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SwitchScene : MonoBehaviour
{
    public static bool checkScene2 = false;
    public static int sc = 0;
    public Image fadeImage; // Image màu đen trong Canvas
    public float fadeDuration = 2.0f; // Thời gian fade
    private bool isSwitching = false; // Biến để kiểm tra quá trình chuyển cảnh

    private void Start()
    {
        // Đảm bảo fadeImage trong suốt lúc đầu
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0);
    }

    private void Update()
    {
        if (sc == 10 && !isSwitching)
        {
            StartCoroutine(FadeAndSwitchScene("Scene2"));
            isSwitching = true; // Đánh dấu rằng quá trình chuyển cảnh đã bắt đầu
        }
    }

    private IEnumerator FadeAndSwitchScene(string newSceneName)
    {
        // Bắt đầu quá trình fade-out (tối dần)
        yield return StartCoroutine(Fade(1.0f));

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
        sc = 100000; // Đặt lại `sc` để không tiếp tục gọi hàm chuyển cảnh trong Update
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
}
