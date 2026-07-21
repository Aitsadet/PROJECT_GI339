using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 3; // เลือดสูงสุดของศัตรู
    private int currentHealth;

    [Header("Animation Names")]
    public string takeHitAnim = "TakeHit_1"; // ชื่อกล่องอนิเมชันตอนโดนตี
    public string dieAnim = "Die_1";         // ชื่อกล่องอนิเมชันตอนตาย

    [Header("Scripts to Disable")]
    public MonoBehaviour patrolScript;       // สคริปต์เดิน (EnemyPatrol)
    public MonoBehaviour attackScript;       // สคริปต์โจมตี (EnemyAttackWarning)

    private Animator animator;
    private bool isDead = false;

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
        }
        else
        {
            StartCoroutine(TakeHitRoutine());
        }
    }

    private IEnumerator TakeHitRoutine()
    {
        // 1. หยุดเดินและหยุดชาร์จโจมตีชั่วคราว
        if (patrolScript != null) patrolScript.enabled = false;
        if (attackScript != null) attackScript.enabled = false;

        // 2. เล่นท่าโดนฟัน
        if (animator != null) animator.Play(takeHitAnim);

        // 3. รอให้ท่าโดนฟันเล่นจบ (ประมาณ 0.4 วินาที)
        yield return new WaitForSeconds(0.4f);

        // 4. ถ้ายังไม่ตาย ให้กลับมาเดินและโจมตีต่อได้
        if (!isDead)
        {
            if (patrolScript != null) patrolScript.enabled = true;
            if (attackScript != null) attackScript.enabled = true;
        }
    }

    private void Die()
    {
        isDead = true;

        // 1. ปิดสคริปต์ทุกอย่างไม่ให้ขยับหรือโจมตีได้อีก
        if (patrolScript != null) patrolScript.enabled = false;
        if (attackScript != null) attackScript.enabled = false;

        // 2. เล่นท่าตาย
        if (animator != null) animator.Play(dieAnim);

        // 3. ปิดกล่องชน (Collider) เพื่อให้ผู้เล่นเดินผ่านศพไปได้ ไม่ติดกำแพงล่องหน
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        // 4. ลบศพทิ้งหลังจากผ่านไป 3 วินาที (ถ้าอยากให้ศพอยู่ตลอดไป ให้ลบบรรทัดนี้ทิ้งครับ)
        Destroy(gameObject, 3f);
    }
}