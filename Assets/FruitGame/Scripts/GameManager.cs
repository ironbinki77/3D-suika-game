using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject pauseMenuCanvas; // PauseMenuCanvas를 연결
    public GameObject winCanvas;      // 승리 UI 캔버스

    private bool isPaused = false; // 현재 일시정지 상태를 추적

    void Update()
    {
        // ESC 키로 일시정지 토글
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        pauseMenuCanvas.SetActive(true); // 일시정지 메뉴 활성화
        Time.timeScale = 0f; // 게임 일시정지

        // 마우스 커서 활성화
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenuCanvas.SetActive(false); // 일시정지 메뉴 비활성화
        Time.timeScale = 1f; // 게임 재개

        // 마우스 커서 비활성화
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // 시간 스케일을 복원
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // 씬 재시작
    }

    public void ReturnToStageMenu()
    {
        Time.timeScale = 1f; // 시간 스케일을 복원
        SceneManager.LoadScene("StageScene"); // StageScene으로 전환
    }

    public void HandleWin()
    {
        // 시간 정지
        Time.timeScale = 0f;
        // 승리 UI 활성화
        if (winCanvas != null)
        {
            winCanvas.SetActive(true);
        }

        // 마우스 커서 활성화
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

}
