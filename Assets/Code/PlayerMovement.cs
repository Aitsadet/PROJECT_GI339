using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("ตั้งค่าการเดิน")]
    public float moveSpeed = 5f;

    [Header("ตั้งค่าการกระโดด")]
    public float jumpForce = 12f;          // แรงกระโดด (ปรับเพิ่มลดได้)
    public Transform groundCheck;         // จุดตรวจจับพื้น (สร้างไว้ใต้เท้า)
    public LayerMask groundLayer;         // เลเยอร์ที่เป็นพื้นฉาก
    public float groundCheckRadius = 0.2f; // รัศมีวงกลมตรวจจับพื้น

    [Header("ส่วนประกอบ (References)")]
    public Rigidbody2D rb;
    public Animator animator;

    private float moveInput;
    private bool isFacingRight = true;
    public bool isGrounded;              // ตัวแปรเช็คว่าอยู่บนพื้นหรือไม่

    void Update()
    {
        // 1. ตรวจจับว่าตัวละครเหยียบพื้นอยู่หรือไม่
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }

        // 2. รับค่าการกดปุ่ม A/D หรือ ลูกศรซ้าย/ขวา
        moveInput = 0f;
        if (Keyboard.current != null)
        {
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            {
                moveInput = 1f;
            }
            else if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            {
                moveInput = -1f;
            }

            // 3. รับคำสั่งกระโดด (ปุ่ม Spacebar) และต้องอยู่บนพื้นเท่านั้นถึงจะกระโดดได้
            if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
        }

        // 4. ส่งค่าไปให้ Animator ควบคุมแอนิเมชัน
        if (animator != null)
        {
            animator.SetFloat("Speed", Mathf.Abs(moveInput));
            animator.SetBool("isGrounded", isGrounded);

            // --- เพิ่มโค้ดบรรทัดนี้ลงไปครับ ---
            animator.SetFloat("yVelocity", rb.velocity.y);
        }

        // 5. ตรวจสอบการหันหน้า
        if (moveInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && isFacingRight)
        {
            Flip();
        }
    }

    void FixedUpdate()
    {
        // 6. สั่งเคลื่อนที่ในระบบฟิสิกส์แกน X (ไม่ยุ่งกับแกน Y เพื่อให้แรงกระโดดทำงานได้ปกติ)
        if (rb != null)
        {
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    // วาดวงกลมตรวจจับพื้นสีเขียวในหน้า Scene เพื่อช่วยกะระยะใต้เท้า
    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}