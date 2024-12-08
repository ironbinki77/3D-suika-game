using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        // 게임 씬으로 전환
        SceneManager.LoadScene("StageScene"); // "StageScene"으로 이동
    }
}