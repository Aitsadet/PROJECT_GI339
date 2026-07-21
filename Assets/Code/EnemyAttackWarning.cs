using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyAttackWarning : MonoBehaviour
{
    [Header("Vision Zone")]
    public BoxCollider2D visionZone;
    public LayerMask playerLayer;

    [Header("Chase Settings")]
    public float chaseSpeed = 2f; // 🌟 ตั้งค่าความเร็วให้มากกว่า 0 (เช่น 2 หรือ 3)
    public string runAnim = "Run_1";

    [Header("Fix Sprite Flip")]
    public bool invertSprite = false;

    [Header("Attack Settings")]
    public float attackTriggerDistance = 1.5f;
    public Vector2 attackTriggerOffset = new Vector2(0f, 0f);
    public float warningTime = 0.8f;
    public float attackRadius = 1.5f;
    public Vector2 attackOffset = new Vector2(1f, 0f);
    public int attackDamage = 20;

    [Header("Animation Names")]
    public string idleAnim = "Idle_0";
    public string walkAnim = "Walk_0";
    public string attackAnim = "Attack_1";

    [Header("Scripts")]
    public MonoBehaviour patrolScript;

    private Transform player;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Color originalColor;
    private bool isAttacking = false;
    private Rigidbody2D rb;
    private bool wasChasing = false;
    private bool isStunned = false;
    private float originalScaleX;
    private float flipCooldown = 0.3f; // 🌟 ตัวแปรหน่วงเวลาไม่ให้หันหน้าเร็วเกินไป
    private float lastFlipTime = 0f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        if (spriteRenderer != null) originalColor = spriteRenderer.color;
        originalScaleX = Mathf.Abs(transform.localScale.x); // ล็อกสเกลตั้งต้นไม่ให้ตัวยืด

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    public void StunEnemy(float duration)
    {
        StartCoroutine(StunRoutine(duration));
    }

    private IEnumerator StunRoutine(float duration)
    {
        isStunned = true;
        if (rb != null) rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        if (animator != null) animator.Play(idleAnim);

        yield return new WaitForSeconds(duration);

        isStunned = false;
    }

    void Update()
    {
        if (player == null || isAttacking || isStunned) return;

        Collider2D playerInSight = null;

        if (visionZone != null)
        {
            playerInSight = Physics2D.OverlapBox(visionZone.bounds.center, visionZone.bounds.size, 0f, playerLayer);
        }

        if (playerInSight != null)
        {
            wasChasing = true;
            if (patrolScript != null) patrolScript.enabled = false;

            Vector2 triggerCenter = (Vector2)transform.position + new Vector2(attackTriggerOffset.x * Mathf.Sign(transform.localScale.x), attackTriggerOffset.y);
            float distanceToPlayer = Vector2.Distance(triggerCenter, player.position);

            if (distanceToPlayer <= attackTriggerDistance)
            {
                StartCoroutine(AttackSequence());
            }
            else
            {
                ChasePlayer();
            }
        }
        else
        {
            if (wasChasing)
            {
                wasChasing = false;
                if (animator != null) animator.Play(walkAnim);
                if (patrolScript != null) patrolScript.enabled = true;
            }
        }
    }

    private void ChasePlayer()
    {
        float xDiff = player.position.x - transform.position.x;
        float distanceToPlayer = Mathf.Abs(xDiff);

        // ถ้าผู้เล่นยังอยู่ห่างพอสมควร ให้วิ่งไล่ตาม
        if (distanceToPlayer > 0.2f)
        {
            float direction = Mathf.Sign(xDiff);
            rb.linearVelocity = new Vector2(direction * chaseSpeed, rb.linearVelocity.y);
            if (animator != null) animator.Play(runAnim);

            // 🌟 ระบบหน่วงเวลาหันหน้า (Cooldown) ป้องกันการสั่นรัวๆ ตอน Dash ผ่านตัว
            if (Time.time >= lastFlipTime + flipCooldown)
            {
                Vector3 newScale = transform.localScale;
                newScale.x = originalScaleX * direction;
                transform.localScale = newScale;
                lastFlipTime = Time.time;
            }
        }
        else
        {
            // ถ้าระยะประชิดแล้ว ให้หยุดเดินนิ่งๆ
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            if (animator != null) animator.Play(idleAnim);
        }
    }

    private IEnumerator AttackSequence()
    {
        isAttacking = true;

        if (rb != null) rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
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
        isAttacking = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector2 triggerCenterGizmo = (Vector2)transform.position + new Vector2(attackTriggerOffset.x * Mathf.Sign(transform.localScale.x), attackTriggerOffset.y);
        Gizmos.DrawWireSphere(triggerCenterGizmo, attackTriggerDistance);

        Gizmos.color = Color.red;
        Vector2 attackPos = (Vector2)transform.position + new Vector2(attackOffset.x * Mathf.Sign(transform.localScale.x), attackOffset.y);
        Gizmos.DrawWireSphere(attackPos, attackRadius);
    }
}