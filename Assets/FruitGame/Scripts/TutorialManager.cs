using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialPanel;    // TutorialPanel 참조
    public Image tutorialImage;         // TutorialImage 참조
    public Sprite[] tutorialSprites;    // 구역별 튜토리얼 이미지 배열
    private Coroutine hideCoroutine;    // 패널 숨김 코루틴
    private bool isPlayerInsideZone = false; // 플레이어가 구역 내에 있는지 추적

    void Start()
    {
        // TutorialPanel 초기 비활성화
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(false);
        }
    }

    public void ShowTutorial(int tutorialIndex)
    {
        // 인덱스 범위 체크
        if (tutorialIndex < 0 || tutorialIndex >= tutorialSprites.Length)
        {
            Debug.LogWarning("Invalid tutorial index!");
            return;
        }

        // 이미지 업데이트
        if (tutorialImage != null)
        {
            tutorialImage.sprite = tutorialSprites[tutorialIndex];
        }

        // 패널 활성화
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(true);
        }

        // 이전 코루틴 취소 (3초 카운트다운 리셋)
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }

        isPlayerInsideZone = true; // 플레이어가 구역 안에 있음
    }

    public void StartHideTutorialTimer(float delay)
    {
        // 플레이어가 구역에 없으면 3초 후 숨김 시작
        isPlayerInsideZone = false;

        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }
        hideCoroutine = StartCoroutine(HideTutorialPanelAfterDelay(delay));
    }

    private IEnumerator HideTutorialPanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // 플레이어가 여전히 구역 안에 있다면 패널 유지
        if (isPlayerInsideZone)
        {
            yield break;
        }

        // 패널 비활성화
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(false);
        }
    }
}
