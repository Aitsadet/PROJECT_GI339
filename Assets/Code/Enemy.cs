using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("ตั้งค่าศัตรู")]
    public int maxHealth = 100; // เลือดสูงสุด
    private int currentHealth;

    void Start()
    {
        // เริ่มเกมมาให้เลือดเต็ม
        currentHealth = maxHealth;
    }

    // ฟังก์ชันนี้จะถูกเรียกใช้จากดาบของผู้เล่น
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("ศัตรูโดนฟัน! เลือดเหลือ: " + currentHealth);

        // คุณสามารถสั่งเล่นแอนิเมชัน Take Hit ตรงนี้ได้ในอนาคต
        // animator.SetTrigger("TakeHit");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("ศัตรูตายแล้ว!");

        // สั่งให้ศัตรูหายไปจากฉาก
        Destroy(gameObject);

        // หากมีแอนิเมชันตาย สามารถเปลี่ยนมาใช้การ Disable Collider และสคริปต์แทนการ Destroy ได้
    }
}