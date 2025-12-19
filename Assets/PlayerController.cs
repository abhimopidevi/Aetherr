using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [Header("Movement")]
    public float moveSpeed = 6f;
    public float jumpForce = 12f;

    [Header("Health")]
    public float maxHealth = 100f;
    public float currentHealth;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool isGrounded;
    private bool isFacingRight = true;
    private float move;
    private bool isDead = false;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (isDead) return;

        if (Input.GetKey(KeyCode.D)) move = 1f;
        else if (Input.GetKey(KeyCode.A)) move = -1f;
        else move = Input.GetAxisRaw("Horizontal");

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump")) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        Flip();
    }

    void FixedUpdate()
    {
        if (isDead) return;
        rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);

        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            checkRadius,
            groundLayer
        );
    }

    void Flip()
    {
        if ((isFacingRight && move < 0f) || (!isFacingRight && move > 0f))
        {
            isFacingRight = !isFacingRight;
            if (sr != null)
                sr.flipX = !isFacingRight;
            else
            {
                Vector3 scale = transform.localScale;
                scale.x *= -1f;
                transform.localScale = scale;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;   
        GameOverManager.Instance.ShowDefeat();
    }
}
