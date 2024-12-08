using UnityEngine;
using UnityEngine.SceneManagement;

public class StageMenuManager : MonoBehaviour
{
    public void StartTest()
    {
        // 게임 씬으로 전환
        SceneManager.LoadScene("TestScene"); // "TestScene"으로 이동
    }

    public void BacktoMain()
    {
        // 메인 씬으로 전환
        SceneManager.LoadScene("MainScene"); // "MainScene"으로 이동
    }
}