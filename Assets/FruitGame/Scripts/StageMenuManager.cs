using UnityEngine;
using UnityEngine.SceneManagement;

public class StageMenuManager : MonoBehaviour
{
    public void StartTest()
    {
        // ���� ������ ��ȯ
        SceneManager.LoadScene("TestScene"); // "TestScene"���� �̵�
    }

    public void BacktoMain()
    {
        // ���� ������ ��ȯ
        SceneManager.LoadScene("MainScene"); // "MainScene"���� �̵�
    }
}