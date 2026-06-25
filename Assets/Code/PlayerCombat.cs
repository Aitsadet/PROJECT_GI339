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
    public AudioSource attackSound; // <-- เพิ่มตัวแปรสำหรับเสียงฟันดาบ

    [Header("ตั้งค่าการต่อสู้ (Combat Settings)")]
    public float attackRange = 0.5f;
    public int attackDamage = 40;
    public float attackRate = 2f;
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

        // 2. เล่นแอนิเมชัน
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        // 3. ทำดาเมจศัตรู
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}