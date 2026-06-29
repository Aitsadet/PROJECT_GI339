using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("ระบบเสียงปุ่มกด")]
    public AudioSource uiAudioSource;
    public AudioClip hoverSound;
    public AudioClip clickSound;      // <-- 1. เพิ่มตัวแปรสำหรับใส่ไฟล์เสียงตอนคลิก

    // ฟังก์ชันเล่นเสียงตอนเมาส์ชี้
    public void PlayHoverSound()
    {
        if (uiAudioSource != null && hoverSound != null)
        {
            uiAudioSource.PlayOneShot(hoverSound);
        }
    }

    // ฟังก์ชันเล่นเสียงตอนกดคลิก
    public void PlayClickSound()
    {
        if (uiAudioSource != null && clickSound != null)
        {
            uiAudioSource.PlayOneShot(clickSound);
        }
    }

    // เมื่อกดปุ่ม Play
    public void PlayGame()
    {
        PlayClickSound(); // สั่งเล่นเสียงคลิกก่อนเลย

        // ใช้คำสั่ง Invoke เพื่อหน่วงเวลา 0.3 วินาที (รอให้เสียงดังจบก่อน แล้วค่อยเรียกฟังก์ชัน LoadScene)
        Invoke("LoadScene", 0.3f);
    }

    // ฟังก์ชันสำหรับเปลี่ยนซีน
    private void LoadScene()
    {
        SceneManager.LoadScene("Scene Demo");
    }

    // เมื่อกดปุ่ม Quit
    public void QuitGame()
    {
        PlayClickSound(); // สั่งเล่นเสียงคลิก
        Debug.Log("ออกจากเกมเรียบร้อย!");

        Invoke("ExitApp", 0.3f); // หน่วงเวลา 0.3 วินาทีก่อนปิดเกม
    }

    // ฟังก์ชันสำหรับปิดเกม
    private void ExitApp()
    {
        Application.Quit();
    }
}