using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu1 : MonoBehaviour
{
    // ฟังก์ชันสำหรับกดเพื่อเปลี่ยนไปหน้า Main Menu (ใส่ชื่อฉากของคุณในเครื่องหมายคำพูด)
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // เปลี่ยนคำว่า MainMenuSceneName เป็นชื่อฉากเมนูของคุณ
    }
}