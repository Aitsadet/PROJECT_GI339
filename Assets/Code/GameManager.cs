using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("หน้าต่าง UI")]
    public GameObject tutorialPanel; // ช่องสำหรับใส่หน้าต่างสอนเล่น

    void Start()
    {
        // 1. ทันทีที่โหลดเข้าฉากนี้มา ให้เปิดหน้าต่างสอนเล่นขึ้นมา
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(true);
        }

        // 2. สั่งหยุดเวลาในเกม (ศัตรูไม่เดิน, ตัวละครตกไม่ถึงพื้น, อนิเมชั่นหยุด)
        Time.timeScale = 0f;
    }

    // ฟังก์ชันนี้จะถูกเรียกใช้เมื่อผู้เล่นกดปุ่ม Play ในหน้าสอนเล่น
    public void StartGame()
    {
        // 1. ซ่อนหน้าต่างสอนเล่น
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(false);
        }

        // 2. สั่งให้เวลาในเกมกลับมาเดินตามปกติ
        Time.timeScale = 1f;
    }
}