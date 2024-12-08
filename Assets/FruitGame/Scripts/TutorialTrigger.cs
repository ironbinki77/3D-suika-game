using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public int tutorialIndex; // ǥ���� Ʃ�丮�� �̹����� �ε���
    private bool playerInside = false; // �÷��̾ ������ �ִ��� ����

    private void OnTriggerEnter(Collider other)
    {
        // �÷��̾ ������ ������ Ʃ�丮�� ǥ��
        if (other.CompareTag("Player"))
        {
            playerInside = true; // �÷��̾ ���� �ȿ� ����
            TutorialManager tutorialManager = FindObjectOfType<TutorialManager>();
            if (tutorialManager != null)
            {
                tutorialManager.ShowTutorial(tutorialIndex);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // �÷��̾ �������� ������ 3�� �� Ʃ�丮�� ����
        if (other.CompareTag("Player"))
        {
            playerInside = false; // �÷��̾ ������ ����
            TutorialManager tutorialManager = FindObjectOfType<TutorialManager>();
            if (tutorialManager != null)
            {
                tutorialManager.StartHideTutorialTimer(3f); // 3�� Ÿ�̸� ����
            }
        }
    }
}
