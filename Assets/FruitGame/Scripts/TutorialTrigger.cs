using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public int tutorialIndex; // 표시할 튜토리얼 이미지의 인덱스
    private bool playerInside = false; // 플레이어가 구역에 있는지 추적

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어가 구역에 들어오면 튜토리얼 표시
        if (other.CompareTag("Player"))
        {
            playerInside = true; // 플레이어가 구역 안에 있음
            TutorialManager tutorialManager = FindObjectOfType<TutorialManager>();
            if (tutorialManager != null)
            {
                tutorialManager.ShowTutorial(tutorialIndex);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 플레이어가 구역에서 나가면 3초 후 튜토리얼 숨김
        if (other.CompareTag("Player"))
        {
            playerInside = false; // 플레이어가 구역을 떠남
            TutorialManager tutorialManager = FindObjectOfType<TutorialManager>();
            if (tutorialManager != null)
            {
                tutorialManager.StartHideTutorialTimer(3f); // 3초 타이머 시작
            }
        }
    }
}
