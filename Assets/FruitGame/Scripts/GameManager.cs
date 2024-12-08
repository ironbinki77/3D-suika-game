using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject pauseMenuCanvas; // PauseMenuCanvas�� ����
    public GameObject winCanvas;      // �¸� UI ĵ����

    private bool isPaused = false; // ���� �Ͻ����� ���¸� ����

    void Update()
    {
        // ESC Ű�� �Ͻ����� ���
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
        pauseMenuCanvas.SetActive(true); // �Ͻ����� �޴� Ȱ��ȭ
        Time.timeScale = 0f; // ���� �Ͻ�����

        // ���콺 Ŀ�� Ȱ��ȭ
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenuCanvas.SetActive(false); // �Ͻ����� �޴� ��Ȱ��ȭ
        Time.timeScale = 1f; // ���� �簳

        // ���콺 Ŀ�� ��Ȱ��ȭ
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // �ð� �������� ����
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // �� �����
    }

    public void ReturnToStageMenu()
    {
        Time.timeScale = 1f; // �ð� �������� ����
        SceneManager.LoadScene("StageScene"); // StageScene���� ��ȯ
    }

    public void HandleWin()
    {
        // �ð� ����
        Time.timeScale = 0f;
        // �¸� UI Ȱ��ȭ
        if (winCanvas != null)
        {
            winCanvas.SetActive(true);
        }

        // ���콺 Ŀ�� Ȱ��ȭ
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

}
