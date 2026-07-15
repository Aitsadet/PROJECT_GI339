using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public string mainMenuSceneName = "MainMenu";

    private bool isPaused = false;

    void Start()
    {
        // Hide the pause menu when the game starts
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
    }

    void Update()
    {
        // Check if the Escape key was pressed
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
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
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f; // Freeze game time
            isPaused = true;
        }
    }

    public void ResumeGame()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f; // Unfreeze game time
            isPaused = false;
        }
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f; // คืนค่าเวลาให้เดินปกติก่อนเริ่มใหม่ (สำคัญมาก!)
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // สั่งโหลดฉากปัจจุบันซ้ำ
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // Make sure time is normal before loading scene
        SceneManager.LoadScene(mainMenuSceneName);
    }
}