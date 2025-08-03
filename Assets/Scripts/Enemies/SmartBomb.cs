using UnityEngine;

public class SmartBomb : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float stopDistance = 0.1f;
    [SerializeField] private float explosionDelay = 0.5f;
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private int damage = 2;
    [SerializeField] private GameObject explosionEffect;

    private Vector2 targetPoint;
    private Rigidbody2D rb;
    private bool isMoving = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetTarget(Vector2 point)
    {
        targetPoint = point;
    }

    private void FixedUpdate()
    {
        if (!isMoving) return;

        Vector2 dir = (targetPoint - (Vector2)transform.position);
        float dist = dir.magnitude;

        if (dist <= stopDistance)
        {
            rb.linearVelocity = Vector2.zero;
            isMoving = false;
            Invoke(nameof(Explode), explosionDelay);
        }
        else
        {
            Vector2 desiredVelocity = dir.normalized * maxSpeed;
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, desiredVelocity, acceleration * Time.fixedDeltaTime);
        }
    }

    private void Explode()
    {
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                hit.GetComponent<PlayerHealth>()?.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
