using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialPanel;    // TutorialPanel ����
    public Image tutorialImage;         // TutorialImage ����
    public Sprite[] tutorialSprites;    // ������ Ʃ�丮�� �̹��� �迭
    private Coroutine hideCoroutine;    // �г� ���� �ڷ�ƾ
    private bool isPlayerInsideZone = false; // �÷��̾ ���� ���� �ִ��� ����

    void Start()
    {
        // TutorialPanel �ʱ� ��Ȱ��ȭ
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(false);
        }
    }

    public void ShowTutorial(int tutorialIndex)
    {
        // �ε��� ���� üũ
        if (tutorialIndex < 0 || tutorialIndex >= tutorialSprites.Length)
        {
            Debug.LogWarning("Invalid tutorial index!");
            return;
        }

        // �̹��� ������Ʈ
        if (tutorialImage != null)
        {
            tutorialImage.sprite = tutorialSprites[tutorialIndex];
        }

        // �г� Ȱ��ȭ
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(true);
        }

        // ���� �ڷ�ƾ ��� (3�� ī��Ʈ�ٿ� ����)
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }

        isPlayerInsideZone = true; // �÷��̾ ���� �ȿ� ����
    }

    public void StartHideTutorialTimer(float delay)
    {
        // �÷��̾ ������ ������ 3�� �� ���� ����
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

        // �÷��̾ ������ ���� �ȿ� �ִٸ� �г� ����
        if (isPlayerInsideZone)
        {
            yield break;
        }

        // �г� ��Ȱ��ȭ
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(false);
        }
    }
}
