using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        // ���� ������ ��ȯ
        SceneManager.LoadScene("StageScene"); // "StageScene"���� �̵�
    }
}