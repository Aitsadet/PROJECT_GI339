using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems; // <-- 1. ต้องเพิ่มบรรทัดนี้ เพื่อใช้ระบบจัดการ UI

public class PlayerCombat : MonoBehaviour
{
    [Header("ส่วนประกอบ (References)")]
    public Animator animator;
    public Transform attackPoint;
    public LayerMask enemyLayers;
    private PlayerMovement playerMovement;

    [Header("ระบบเสียง (Audio)")]
    public AudioSource attackSound;

    [Header("ตั้งค่าการต่อสู้ (Combat Settings)")]
    public float attackRange = 0.5f;
    public int attackDamage = 40;
    public float attackRate = 2f;

    [Header("ความหน่วงของดาเมจ (วินาที)")]
    public float damageDelay = 0.2f;

    private float nextAttackTime = 0f;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            // รับคำสั่งคลิกซ้าย
            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            {
                // --- จุดที่แก้ไข (ป้องกันคลิกทะลุ UI) ---
                // ถ้าในฉากมี EventSystem และเมาส์กำลังชี้อยู่บน UI (เช่น ปุ่ม Play)
                if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                {
                    return; // สั่ง return เพื่อให้ออกจากฟังก์ชันนี้ไปเลย (ไม่ทำคำสั่งฟันดาบด้านล่างต่อ)
                }
                // ------------------------------------

                if (playerMovement != null && playerMovement.isGrounded == true)
                {
                    Attack();
                    nextAttackTime = Time.time + 1f / attackRate;
                }
            }
        }
    }

    void Attack()
    {
        if (attackSound != null)
        {
            attackSound.Play();
        }

        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        StartCoroutine(DealDamageAfterDelay());
    }

    IEnumerator DealDamageAfterDelay()
    {
        yield return new WaitForSeconds(damageDelay);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            // ถ้าศัตรูมีสคริปต์ Enemy อยู่ ให้สั่งลดเลือด (ตรวจสอบชื่อคลาส Enemy ของคุณด้วยนะ)
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(attackDamage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}