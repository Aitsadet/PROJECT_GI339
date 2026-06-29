using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("ตั้งค่าการเดิน")]
    public float moveSpeed = 5f;

    [Header("ตั้งค่าการกระโดด")]
    public float jumpForce = 12f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;

    [Header("ตั้งค่าการ Dash")]
    public float dashPower = 24f;
    public float dashTime = 0.2f;
    public float dashCooldown = 1f;
    private bool canDash = true;
    private bool isDashing;

    [Header("เอฟเฟกต์ (Effects)")]
    public TrailRenderer trailRenderer;

    [Header("ระบบเสียง (Audio)")]
    public AudioSource footstepAudioSource;
    public AudioSource sfxAudioSource;
    public AudioClip jumpSound;
    public AudioClip dashSound;             // <-- 1. เพิ่มช่องใส่ไฟล์เสียง Dash ตรงนี้

    [Header("ส่วนประกอบ (References)")]
    public Rigidbody2D rb;
    public Animator animator;

    private float moveInput;
    private bool isFacingRight = true;
    public bool isGrounded;

    void Update()
    {
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }

        if (!isDashing)
        {
            moveInput = 0f;
            if (Keyboard.current != null)
            {
                if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                    moveInput = 1f;
                else if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
                    moveInput = -1f;

                if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);

                    if (sfxAudioSource != null && jumpSound != null)
                    {
                        sfxAudioSource.PlayOneShot(jumpSound);
                    }
                }

                if (Keyboard.current.leftShiftKey.wasPressedThisFrame && canDash)
                    StartCoroutine(DashCoroutine());
            }

            if (moveInput != 0 && isGrounded)
            {
                if (footstepAudioSource != null && !footstepAudioSource.isPlaying)
                {
                    footstepAudioSource.Play();
                }
            }
            else
            {
                if (footstepAudioSource != null) footstepAudioSource.Stop();
            }

            if (moveInput > 0 && !isFacingRight)
                Flip();
            else if (moveInput < 0 && isFacingRight)
                Flip();
        }
        else
        {
            if (footstepAudioSource != null) footstepAudioSource.Stop();
        }

        if (animator != null)
        {
            animator.SetFloat("Speed", Mathf.Abs(moveInput));
            animator.SetBool("isGrounded", isGrounded);
            animator.SetFloat("yVelocity", rb.velocity.y);
        }
    }

    void FixedUpdate()
    {
        if (isDashing) return;

        if (rb != null)
        {
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        }
    }

    private IEnumerator DashCoroutine()
    {
        canDash = false;
        isDashing = true;

        // --- 2. สั่งเล่นเสียง Dash ทันทีที่เริ่มพุ่งตัว ---
        if (sfxAudioSource != null && dashSound != null)
        {
            sfxAudioSource.PlayOneShot(dashSound);
        }
        // ------------------------------------------

        if (trailRenderer != null) trailRenderer.emitting = true;

        int playerLayer = gameObject.layer;
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

        float dashDirection = isFacingRight ? 1f : -1f;
        rb.velocity = new Vector2(dashDirection * dashPower, 0f);

        yield return new WaitForSeconds(dashTime);

        if (trailRenderer != null) trailRenderer.emitting = false;

        rb.gravityScale = originalGravity;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);

        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
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
        if (groundCheck == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}