using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextEffect : MonoBehaviour
{
    public TextMeshProUGUI textComponent; // Text component để hiển thị chữ (hoặc TextMeshPro)
    public float typingSpeed = 0.05f; // Thời gian mỗi ký tự xuất hiện (tốc độ gõ)

    private string fullText = "Dù phải đốt cháy cả dãy Trường Sơn cũng phải giành cho được độc lập!";

    void Start()
    {
        // Bắt đầu Coroutine để hiển thị chữ dần dần
        StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        textComponent.text = ""; // Đảm bảo textComponent bắt đầu trống

        foreach (char letter in fullText)
        {
            textComponent.text += letter; // Thêm từng ký tự vào Text
            yield return new WaitForSeconds(typingSpeed); // Đợi một thời gian trước khi thêm ký tự tiếp theo
        }
    }
}
