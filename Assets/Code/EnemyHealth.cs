using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 3;
    private int currentHealth;

    [Header("Animation Names")]
    public string takeHitAnim = "TakeHit_1";
    public string dieAnim = "Die_1";

    [Header("Scripts to Disable")]
    public MonoBehaviour patrolScript;
    public MonoBehaviour attackScript;

    [Header("Goal Door Reference")]
    public GameObject goalDoor; // ลากประตูมาใส่ในช่องนี้ผ่าน Inspector

    private Animator animator;
    private bool isDead = false;

    // 🌟 เพิ่มตัวแปรเช็คสถานะการโดนตี เพื่อป้องกันบั๊กโดนฟันรัวๆ แล้วทำงานซ้อนกัน
    private Coroutine hitCoroutine;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        // 🌟 ถ้ายืนชะงักอยู่แล้วโดนฟันซ้ำ ให้เคลียร์ของเก่าทิ้งแล้วนับเวลาชะงักใหม่
        if (hitCoroutine != null)
        {
            StopCoroutine(hitCoroutine);
        }

        // สั่งให้สคริปต์โจมตีรับรู้ว่าโดนชะงัก
        EnemyAttackWarning warningScript = GetComponent<EnemyAttackWarning>();
        if (warningScript != null)
        {
            warningScript.StunEnemy(0.4f);
        }

        hitCoroutine = StartCoroutine(TakeHitRoutine());
    }

    private IEnumerator TakeHitRoutine()
    {
        // 1. หยุดสคริปต์ AI ทุกตัวทันที
        if (patrolScript != null) patrolScript.enabled = false;
        if (attackScript != null) attackScript.enabled = false;

        // 2. หยุดแรงเหวี่ยงฟิสิกส์ให้ตัวหยุดนิ่ง
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

        // 3. เล่นท่าโดนตี
        if (animator != null)
        {
            animator.Play(takeHitAnim, 0, 0f);
        }

        // 4. รอจนกว่าจะเล่นท่าเสร็จ
        yield return new WaitForSeconds(0.4f);

        // 5. คืนชีพ AI
        if (!isDead)
        {
            // เปิดสคริปต์ไล่ล่าทำงานต่อ
            if (attackScript != null) attackScript.enabled = true;

            // ❌ เอาโค้ดที่เปิด patrolScript ทิ้งไปเลย เพื่อป้องกันสคริปต์แย่งกันทำงาน! 
            // ให้ attackScript เป็นคนตัดสินใจเองว่าจะเปิด Patrol ตอนที่ผู้เล่นเดินหนีไปแล้ว

            // ยกเว้นกรณีไม่มี attackScript ให้เปิด patrol ปกติ
            if (attackScript == null && patrolScript != null)
            {
                patrolScript.enabled = true;
            }
        }
    }

    private void Die()
    {
        isDead = true;
        if (patrolScript != null) patrolScript.enabled = false;
        if (attackScript != null) attackScript.enabled = false;
        if (animator != null) animator.Play(dieAnim);

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        // 🌟 สั่งให้ประตูโผล่ขึ้นมา (เปิดใช้งาน GameObject ของประตู) เมื่อบอสตาย
        if (goalDoor != null)
        {
            goalDoor.SetActive(true);
        }

        Destroy(gameObject, 0.5f);
    }
}