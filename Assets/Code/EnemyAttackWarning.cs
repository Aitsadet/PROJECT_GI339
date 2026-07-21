using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyAttackWarning : MonoBehaviour
{
    [Header("Attack Settings")]
    public float triggerDistance = 3f;
    public float warningTime = 0.8f;
    public float attackRadius = 1.5f;
    public Vector2 attackOffset = new Vector2(1f, 0f);
    public LayerMask playerLayer;
    public int attackDamage = 20; // <-- เพิ่มพลังโจมตีของศัตรูตรงนี้!

    [Header("Animation Names")]
    public string idleAnim = "Idle_0";
    public string attackAnim = "Attack_1";

    [Header("Scripts")]
    public MonoBehaviour patrolScript;

    private Transform player;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Color originalColor;
    private bool isAttacking = false;
    private Rigidbody2D rb;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        if (spriteRenderer != null) originalColor = spriteRenderer.color;

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    void Update()
    {
        if (player == null || isAttacking) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= triggerDistance)
        {
            float directionToPlayer = Mathf.Sign(player.position.x - transform.position.x);
            float currentFacing = Mathf.Sign(transform.localScale.x);

            if (directionToPlayer == currentFacing)
            {
                StartCoroutine(AttackSequence());
            }
        }
    }

    private IEnumerator AttackSequence()
    {
        isAttacking = true;

        if (patrolScript != null) patrolScript.enabled = false;
        if (rb != null) rb.velocity = Vector2.zero;
        if (animator != null) animator.Play(idleAnim);
        if (spriteRenderer != null) spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(warningTime);

        if (spriteRenderer != null) spriteRenderer.color = originalColor;
        if (animator != null) animator.Play(attackAnim);

        yield return new WaitForSeconds(0.3f);

        Vector2 attackPos = (Vector2)transform.position + new Vector2(attackOffset.x * Mathf.Sign(transform.localScale.x), attackOffset.y);
        Collider2D hitPlayer = Physics2D.OverlapCircle(attackPos, attackRadius, playerLayer);

        if (hitPlayer != null)
        {
            // --- เปลี่ยนจากฆ่าทันที เป็นการสั่งลดเลือด ---
            PlayerHealth pHealth = hitPlayer.GetComponent<PlayerHealth>();
            if (pHealth != null)
            {
                pHealth.TakeDamage(attackDamage);
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        yield return new WaitForSeconds(0.5f);

        if (patrolScript != null) patrolScript.enabled = true;

        yield return new WaitForSeconds(1f);
        isAttacking = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, triggerDistance);

        Gizmos.color = Color.red;
        Vector2 attackPos = (Vector2)transform.position + new Vector2(attackOffset.x * Mathf.Sign(transform.localScale.x), attackOffset.y);
        Gizmos.DrawWireSphere(attackPos, attackRadius);
    }
}