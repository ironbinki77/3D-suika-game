using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    public Image fadeImage; // 페이드 효과를 위한 UI 이미지
    public float slowMotionScale = 0.2f; // 슬로우 모션 속도

    // 페이드 아웃 (단순 호출)
    public void FadeOut(float duration)
    {
        StartCoroutine(FadeOutCoroutine(duration));
    }

    // 페이드 아웃 완료까지 기다릴 수 있는 메서드
    public IEnumerator FadeOutAndWait(float duration)
    {
        yield return FadeOutCoroutine(duration);
    }

    private IEnumerator FadeOutCoroutine(float duration)
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;

        // 슬로우 모션 시작
        Time.timeScale = slowMotionScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale; // FixedUpdate의 시간 스케일 조정

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime; // Time.timeScale 영향을 받지 않음
            color.a = Mathf.Lerp(0f, 1f, elapsedTime / duration); // 알파값 증가
            fadeImage.color = color;
            yield return null;
        }

        color.a = 1f;
        fadeImage.color = color;

        // 슬로우 모션 종료
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f; // FixedUpdate의 기본값 복원
    }
}
