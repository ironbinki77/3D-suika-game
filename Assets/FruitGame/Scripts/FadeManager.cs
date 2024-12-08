using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    public Image fadeImage; // ���̵� ȿ���� ���� UI �̹���
    public float slowMotionScale = 0.2f; // ���ο� ��� �ӵ�

    // ���̵� �ƿ� (�ܼ� ȣ��)
    public void FadeOut(float duration)
    {
        StartCoroutine(FadeOutCoroutine(duration));
    }

    // ���̵� �ƿ� �Ϸ���� ��ٸ� �� �ִ� �޼���
    public IEnumerator FadeOutAndWait(float duration)
    {
        yield return FadeOutCoroutine(duration);
    }

    private IEnumerator FadeOutCoroutine(float duration)
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;

        // ���ο� ��� ����
        Time.timeScale = slowMotionScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale; // FixedUpdate�� �ð� ������ ����

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime; // Time.timeScale ������ ���� ����
            color.a = Mathf.Lerp(0f, 1f, elapsedTime / duration); // ���İ� ����
            fadeImage.color = color;
            yield return null;
        }

        color.a = 1f;
        fadeImage.color = color;

        // ���ο� ��� ����
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f; // FixedUpdate�� �⺻�� ����
    }
}
