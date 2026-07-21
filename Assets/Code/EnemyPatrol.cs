using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public bool isFacingRight = true;

    [Header("Sensors")]
    public Transform edgeCheck;
    public LayerMask groundLayer;
    public float edgeCheckDistance = 0.5f;

    [Header("Wall Check Settings")]
    public float wallCheckDistance = 0.5f;

    private Rigidbody2D rb;
    private Animator animator;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // This runs when the script gets activated (Start of game, or after attacking)
    void OnEnable()
    {
        if (animator != null)
        {
            animator.Play("Run_1");
        }
    }

    void Update()
    {
        if (edgeCheck == null) return;

        // 1. Ledge check
        RaycastHit2D groundInfo = Physics2D.Raycast(edgeCheck.position, Vector2.down, edgeCheckDistance, groundLayer);

        // 2. Wall check
        Vector2 forwardDirection = isFacingRight ? Vector2.right : Vector2.left;
        RaycastHit2D wallInfo = Physics2D.Raycast(edgeCheck.position, forwardDirection, wallCheckDistance, groundLayer);

        // 3. Flip condition
        if (groundInfo.collider == null || wallInfo.collider != null)
        {
            Flip();
        }
    }

    void FixedUpdate()
    {
        float currentSpeed = isFacingRight ? moveSpeed : -moveSpeed;
        if (rb != null)
        {
            rb.velocity = new Vector2(currentSpeed, rb.velocity.y);
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    void OnDrawGizmosSelected()
    {
        if (edgeCheck == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(edgeCheck.position, edgeCheck.position + Vector3.down * edgeCheckDistance);

        Gizmos.color = Color.blue;
        Vector3 forwardDirection = isFacingRight ? Vector3.right : Vector3.left;
        Gizmos.DrawLine(edgeCheck.position, edgeCheck.position + forwardDirection * wallCheckDistance);
    }
}