using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 12f;
    public float lifetime = 3f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 dir)
    {
        rb.linearVelocity = dir.normalized * speed;
    }

    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0f)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        BossAI boss = other.GetComponent<BossAI>();
        if (boss != null)
        {
            boss.TakeDamage(20f);
            Destroy(gameObject);
        }
    }
}

