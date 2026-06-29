using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("ตั้งค่าการเดิน")]
    public float moveSpeed = 2f;
    public bool isFacingRight = true;

    [Header("ระบบเซ็นเซอร์ (Sensors)")]
    public Transform edgeCheck;            // จุดอ้างอิงที่อยู่ด้านหน้าตัวศัตรู
    public LayerMask groundLayer;          // เลเยอร์ของพื้นและกำแพง
    public float edgeCheckDistance = 0.5f; // ความยาวเส้นแดง (เช็คเหว)

    [Header("เพิ่มความยาวเส้นเช็คกำแพง")]
    public float wallCheckDistance = 0.5f; // <-- เพิ่มตัวแปรนี้ ความยาวเส้นน้ำเงิน (เช็คกำแพง/พื้นสูง)

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (edgeCheck == null) return;

        // 1. เส้นสีแดง: ยิงลงข้างล่างเพื่อเช็คเหว
        RaycastHit2D groundInfo = Physics2D.Raycast(edgeCheck.position, Vector2.down, edgeCheckDistance, groundLayer);

        // 2. เส้นสีน้ำเงิน: ยิงไปข้างหน้าเพื่อเช็คกำแพง หรือพื้นสูงขวางหน้า
        Vector2 forwardDirection = isFacingRight ? Vector2.right : Vector2.left;
        RaycastHit2D wallInfo = Physics2D.Raycast(edgeCheck.position, forwardDirection, wallCheckDistance, groundLayer);

        // 3. ถ้าตกเหว (แดงไม่โดนพื้น) หรือ ชนกำแพง/พื้นสูง (น้ำเงินชนพื้น) ให้หันหลัง
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

        // วาดเส้นสีแดง (เช็คเหว)
        Gizmos.color = Color.red;
        Gizmos.DrawLine(edgeCheck.position, edgeCheck.position + Vector3.down * edgeCheckDistance);

        // วาดเส้นสีน้ำเงิน (เช็คกำแพง/พื้นสูง)
        Gizmos.color = Color.blue;
        Vector3 forwardDirection = isFacingRight ? Vector3.right : Vector3.left;
        Gizmos.DrawLine(edgeCheck.position, edgeCheck.position + forwardDirection * wallCheckDistance);
    }
}