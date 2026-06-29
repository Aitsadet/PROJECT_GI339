using UnityEngine;
using UnityEngine.SceneManagement; // 🚨 บรรทัดสำคัญ: จำเป็นต้องใส่เพื่อใช้งานคำสั่งเกี่ยวกับฉาก
using UnityEngine.InputSystem;

public class Portal : MonoBehaviour
{
    public string nextSceneName; // ตัวแปรสำหรับพิมพ์ชื่อด่านต่อไปใน Unity Inspector

    private bool isPlayerInZone = false;

    void Update()
    {
        // ตรวจสอบว่าผู้เล่นเดินมาอยู่ตรงประตูและกดปุ่ม E หรือไม่
        if (isPlayerInZone && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            // คำสั่งหลักที่สั่งให้ Unity ปิดด่านเก่าและเปิดด่านใหม่ทันที
            SceneManager.LoadScene(nextSceneName);
        }
    }

    // ระบบจะทำงานเมื่อตัวละครเดินเข้ามาสัมผัสกรอบประตู
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
        }
    }

    // ระบบจะทำงานเมื่อตัวละครเดินออกจากกรอบประตู
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
        }
    }
}