using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PlayerCombat : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public Transform attackPoint;
    public LayerMask enemyLayers;
    private PlayerMovement playerMovement;

    [Header("Audio")]
    public AudioSource attackSound;

    [Header("Combat Settings")]
    public float attackRange = 0.5f;
    public int attackDamage = 40;
    public float attackRate = 2f;

    [Header("Damage Delay (Seconds)")]
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
            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            {
                // Prevent UI click-through
                if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }

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
            // 1. Try to deal damage to the NEW EnemyHealth script (Goblin)
            EnemyHealth newEnemyScript = enemy.GetComponent<EnemyHealth>();
            if (newEnemyScript != null)
            {
                newEnemyScript.TakeDamage(attackDamage);
            }
            else
            {
                // 2. Fallback: Try to deal damage to the OLD Enemy script (if any)
                Enemy oldEnemyScript = enemy.GetComponent<Enemy>();
                if (oldEnemyScript != null)
                {
                    oldEnemyScript.TakeDamage(attackDamage);
                }
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