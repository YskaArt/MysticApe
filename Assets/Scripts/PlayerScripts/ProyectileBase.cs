using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private int damage = 1;

    private Vector2 direction;

    private void Start()
    {
        GetComponent<Rigidbody2D>().linearVelocity = direction * speed;
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        Destroy(gameObject, lifetime);
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyBase>()?.TakeDamage(damage);

            Destroy(gameObject);
        }
    }
}
