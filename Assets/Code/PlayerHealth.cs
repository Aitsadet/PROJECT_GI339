using UnityEngine;
using UnityEngine.UI; // 🌟 1. เพิ่มบรรทัดนี้เพื่อเรียกใช้ UI
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Player Stats")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI Reference")]
    public Slider hpSlider; // 🌟 2. ช่องสำหรับลาก Slider เลือดมาใส่

    [Header("Animator Reference")]
    public Animator animator;

    void Start()
    {
        currentHealth = maxHealth;

        // 🌟 3. เซ็ตค่าเริ่มต้นให้ Slider เลือดเต็ม
        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHealth;
            hpSlider.value = currentHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // ป้องกันเลือดติดลบเกินจริง
        Debug.Log("Player HP: " + currentHealth);

        // 🌟 4. อัปเดตค่าหลอดเลือดบน UI ทันทีที่เลือดลด
        if (hpSlider != null)
        {
            hpSlider.value = currentHealth;
        }

        if (animator != null && currentHealth > 0)
        {
            animator.SetTrigger("TakeHit");
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player Died!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}