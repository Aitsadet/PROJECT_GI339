using UnityEngine;

public class Lava : MonoBehaviour
{
    public int damage = 100; // ปริมาณดาเมจที่จะทำกับผู้เล่น

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // เช็คว่าสิ่งที่ชนคือ Player หรือไม่
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage); // หักเลือด 100
            }
        }
    }
}