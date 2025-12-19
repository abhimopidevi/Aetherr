using UnityEngine;

public class BossAI : MonoBehaviour
{
    [Header("Stats")]
    public float maxHealth = 200f;
    public float currentHealth;

    [Header("Movement")]
    public float moveSpeed = 3f;
    public float chaseDistance = 8f;

    [Header("Jump / Slam Attack")]
    public float jumpForce = 12f;
    public float slamRadius = 2f;
    public LayerMask playerLayer;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Slam Settings")]
    public float slamCooldown = 1f;
    private float slamTimer = 0f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Transform player;

    private bool isGrounded;
    private bool isJumping;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        currentHealth = maxHealth;
        player = PlayerController.Instance.transform;
    }

    void Update()
    {
        if (currentHealth <= 0) return;

        // Ground check
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        slamTimer -= Time.deltaTime;

        float distance = Vector2.Distance(transform.position, player.position);

        if (isGrounded)
        {
            if (isJumping)
            {
                // Just landed
                isJumping = false;
                DoSlam();
            }
            else
            {
                if (distance <= chaseDistance)
                {
                    ChasePlayer();
                }

                if (distance <= chaseDistance && slamTimer <= 0f && !isJumping)
                {
                    JumpAttack();
                }
            }
        }
    }

    void ChasePlayer()
    {
        float direction = Mathf.Sign(player.position.x - transform.position.x);

        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

        sr.flipX = direction < 0;
    }

    void JumpAttack()
    {
        isJumping = true;
        slamTimer = slamCooldown;

        float direction = Mathf.Sign(player.position.x - transform.position.x);

        rb.linearVelocity = new Vector2(
            direction * moveSpeed,
            jumpForce
        );
    }

    void DoSlam()
    {
        Collider2D hit = Physics2D.OverlapCircle(
            transform.position,
            slamRadius,
            playerLayer
        );

        if (hit != null)
        {
            PlayerController player = hit.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(25f);
            }

        }
    }

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        Debug.Log("Boss defeated!");
        GameOverManager.Instance.ShowVictory();
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, slamRadius);

        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
