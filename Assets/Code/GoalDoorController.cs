using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GoalDoorController : MonoBehaviour
{
    public GameObject endGameUI;
    public string mainMenuSceneName = "MainMenu"; // ชื่อซีนหน้าเมนูของคุณ
    private bool isPlayerInZone = false;

    void Update()
    {
        if (isPlayerInZone && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (endGameUI != null)
            {
                endGameUI.SetActive(true); // เปิดหน้าจอ The End
                Time.timeScale = 0f; // หยุดเวลาในเกม
            }
        }
    }

    // ฟังก์ชันนี้เอาไว้ให้ปุ่ม Main Menu เรียกใช้
    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // คืนค่าเวลาให้เดินปกติก่อนเปลี่ยนฉาก
        SceneManager.LoadScene(mainMenuSceneName);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
        }
    }
}