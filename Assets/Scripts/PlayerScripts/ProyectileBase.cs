using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    [Header("Propiedades Generales")]
    [SerializeField] protected float speed = 10f;
    [SerializeField] protected float lifetime = 3f;
    [SerializeField] protected int damage = 1;

    protected PlayerController.Element element;
    protected Vector2 direction;

    protected Rigidbody2D rb;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * speed;
        Destroy(gameObject, lifetime);
    }

    public virtual void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public virtual void SetElement(PlayerController.Element e)
    {
        element = e;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        // Interacción con enemigos
        if (collision.CompareTag("Enemy") && collision.TryGetComponent(out EnemyBase enemy))
        {
            enemy.TakeDamage(damage, element);
            OnHit();
        }

        // Interacción con Dummy
        if (collision.TryGetComponent(out TrainingDummy dummy))
        {
            dummy.Hit(element);
            OnHit();
        }
    }

    protected virtual void OnHit()
    {
        Destroy(gameObject);
    }
}
