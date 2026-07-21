using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Player Stats")]
    public int maxHealth = 100;
    public int currentHealth;

    void Start()
    {
        // เริ่มเกมมาให้เลือดเต็ม 100
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Player HP: " + currentHealth);

        // ถ้าเลือดหมด (เหลือน้อยกว่าหรือเท่ากับ 0)
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player Died!");
        // รีสตาร์ทด่านใหม่เมื่อตาย (ถ้ามีท่าตาย ค่อยมาใส่เพิ่มทีหลังได้ครับ)
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}