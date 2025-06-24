using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] protected int maxHealth = 3;
    protected int currentHealth;

    [Header("Movimiento")]
    [SerializeField] protected float moveSpeed = 2f;

    [Header("Referencias")]
    protected Rigidbody2D rb;
    protected Animator animator;
    protected Transform player;

    protected enum EnemyState
    {
        Idle, Patrol, Attack, Dead
    }
    protected EnemyState currentState;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        currentState = EnemyState.Idle;
    }

    protected virtual void Update()
    {
        UpdateState();
    }

    protected abstract void UpdateState();

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
       
    }

    protected virtual void Die()
    {
        currentState = EnemyState.Dead;
        animator?.SetTrigger("Die");

        // Desactivar colisiomn y movimiento
        GetComponent<Collider2D>().enabled = false;
        rb.linearVelocity = Vector2.zero;

        // Destruir tras undelay
        Destroy(gameObject, 1.5f);
    }

    protected virtual void MoveTowardsPlayer()
    {
        if (player == null || currentState == EnemyState.Dead)
            return;

        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;

        
    }

    public virtual void Attack()
    {
    }
}
