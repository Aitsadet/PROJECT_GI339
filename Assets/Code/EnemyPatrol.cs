using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("ตั้งค่าการเดิน")]
    public float moveSpeed = 2f;
    public bool isFacingRight = true;

    [Header("ระบบตรวจจับขอบพื้น (Edge Check)")]
    public Transform edgeCheck;        // จุดอ้างอิงที่อยู่ด้านหน้าตัวศัตรู
    public LayerMask groundLayer;      // เลเยอร์ของพื้น
    public float checkDistance = 0.5f; // ความยาวของเส้นเช็คเหว

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (edgeCheck == null) return;

        // 1. ยิงเส้นเลเซอร์จำลองลงไปข้างล่าง เพื่อเช็คว่ามีพื้นให้เหยียบไหม (กันตกเหว)
        RaycastHit2D groundInfo = Physics2D.Raycast(edgeCheck.position, Vector2.down, checkDistance, groundLayer);

        // 2. ยิงเส้นเลเซอร์ไปข้างหน้า เพื่อเช็คว่าชนกำแพงไหม
        Vector2 forwardDirection = isFacingRight ? Vector2.right : Vector2.left;
        RaycastHit2D wallInfo = Physics2D.Raycast(edgeCheck.position, forwardDirection, 0.1f, groundLayer);

        // 3. ถ้าเส้นเลเซอร์ชี้ลงไปแล้วไม่เจอพื้น (สุดขอบ) หรือ ชนกำแพงด้านหน้า ให้หันหลังกลับ
        if (groundInfo.collider == null || wallInfo.collider != null)
        {
            Flip();
        }
    }

    void FixedUpdate()
    {
        // 4. สั่งให้ศัตรูเดินไปข้างหน้าตามทิศทางที่หันอยู่
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
        localScale.x *= -1f; // กลับด้านแกน X เพื่อพลิกซ้ายขวา
        transform.localScale = localScale;
    }

    // วาดเส้นสีแดงและสีน้ำเงินในหน้า Scene เพื่อให้กะระยะง่ายขึ้น
    void OnDrawGizmosSelected()
    {
        if (edgeCheck == null) return;

        // วาดเส้นเช็คขอบเหว (สีแดงชี้ลงล่าง)
        Gizmos.color = Color.red;
        Gizmos.DrawLine(edgeCheck.position, edgeCheck.position + Vector3.down * checkDistance);

        // วาดเส้นเช็คกำแพง (สีน้ำเงินชี้ไปข้างหน้า)
        Gizmos.color = Color.blue;
        Vector3 forwardDirection = isFacingRight ? Vector3.right : Vector3.left;
        Gizmos.DrawLine(edgeCheck.position, edgeCheck.position + forwardDirection * 0.1f);
    }
}