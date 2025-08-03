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
        Idle, Patrol,Chase, Attack, Dead
    }
    [SerializeField] protected EnemyState currentState;
    protected Vector2 moveDirection;
    protected Vector2 lastMoveDir = Vector2.down;

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
        if (currentState != EnemyState.Dead)
            UpdateState();

        UpdateAnimator();
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
        animator.SetBool("IsDeath", true);
        GetComponent<Collider2D>().enabled = false;
        rb.linearVelocity = Vector2.zero;
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
    protected void UpdateAnimator()
    {
        animator.SetFloat("MoveX", moveDirection.x);
        animator.SetFloat("MoveY", moveDirection.y);
        animator.SetBool("IsMoving", moveDirection != Vector2.zero);

        animator.SetFloat("LastMoveX", lastMoveDir.x);
        animator.SetFloat("LastMoveY", lastMoveDir.y);
    }

}
