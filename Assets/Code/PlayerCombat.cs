using System.Collections; // <-- ต้องมีอันนี้ เพื่อใช้ระบบหน่วงเวลา
using UnityEngine;
using UnityEngine.InputSystem;

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
    public float damageDelay = 0.2f; // <-- เพิ่มตัวแปรนี้เข้ามา ปรับค่าใน Unity ได้

    private float nextAttackTime = 0f;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            {
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
        // 1. สั่งเล่นเสียงฟันดาบ
        if (attackSound != null)
        {
            attackSound.Play();
        }

        // 2. เล่นแอนิเมชันง้างดาบ
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        // 3. สั่งให้เริ่มการหน่วงเวลา ก่อนจะทำดาเมจ
        StartCoroutine(DealDamageAfterDelay());
    }

    // ฟังก์ชันพิเศษสำหรับหน่วงเวลา (Coroutine)
    IEnumerator DealDamageAfterDelay()
    {
        // รอเวลาตามที่เราตั้งค่าไว้ใน Inspector (เช่น 0.2 วินาที)
        yield return new WaitForSeconds(damageDelay);

        // หลังจากรอเสร็จแล้ว ค่อยสร้างวงกลมเช็คการโดนตัวศัตรู
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            // ถ้าศัตรูมีสคริปต์ Enemy อยู่ ให้สั่งลดเลือด
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