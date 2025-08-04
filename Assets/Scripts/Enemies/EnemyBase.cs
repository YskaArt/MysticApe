using UnityEngine;
using System.Collections;

public abstract class EnemyBase : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] protected int maxHealth = 3;
    protected int currentHealth;
    [SerializeField] protected int enemyScoreValue;

    [Header("Movimiento")]
    [SerializeField] protected float moveSpeed = 2f;

    [Header("Referencias")]
    [SerializeField] protected SpriteRenderer enemySprite;
    protected Rigidbody2D rb;
    protected Animator animator;
    protected Transform player;

    private Coroutine colorCoroutine;
    private Color baseColor = Color.white;
    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;

    protected enum EnemyState
    {
        Idle, Patrol, Chase, Attack, Dead
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

    public virtual void TakeDamage(int damage, PlayerController.Element sourceElement)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} fue golpeado por {sourceElement}");

        switch (sourceElement)
        {
            case PlayerController.Element.Planta:
                StartCoroutine(ApplySlowEffect(0.5f, 2f));
                StartCoroutine(ChangeColorTemporarily(Color.green, 0.3f));
                break;

            case PlayerController.Element.Roca:
                Vector2 knockbackDir = (transform.position - player.position).normalized;
                rb.AddForce(knockbackDir * 500f);
                StartCoroutine(ChangeColorTemporarily(Color.gray, 0.2f));
                break;

            case PlayerController.Element.Fuego:
                StartCoroutine(BurnEffect(1, 1f));
                StartCoroutine(ChangeColorTemporarily(Color.red, 0.3f));
                break;

            case PlayerController.Element.Hielo:
                StartCoroutine(FreezeEffect(1f));
                StartCoroutine(ChangeColorTemporarily(Color.cyan, 1f));
                break;
        }

        if (currentHealth <= 0)
            Die();
    }

    protected virtual void Die()
    {
        currentState = EnemyState.Dead;
        animator.SetBool("IsDeath", true);
        GetComponent<Collider2D>().enabled = false;
        rb.linearVelocity = Vector2.zero;
        ScoreManager.Instance.AddScore(enemyScoreValue);
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

    protected virtual void UpdateAnimator()
    {
        animator.SetFloat("MoveX", moveDirection.x);
        animator.SetFloat("MoveY", moveDirection.y);
        animator.SetBool("IsMoving", moveDirection != Vector2.zero);

        animator.SetFloat("LastMoveX", lastMoveDir.x);
        animator.SetFloat("LastMoveY", lastMoveDir.y);
    }

    // Efecto Planta: ralentizar
    protected IEnumerator ApplySlowEffect(float speedMultiplier, float duration)
    {
        float originalSpeed = moveSpeed;
        moveSpeed *= speedMultiplier;

        if (enemySprite != null)
            enemySprite.color = Color.green;

        yield return new WaitForSeconds(duration);

        moveSpeed = originalSpeed;

        if (enemySprite != null)
            enemySprite.color = Color.white; // o el color base original
    }

    // Efecto Hielo: congelar
    protected IEnumerator FreezeEffect(float duration)
    {
        float originalSpeed = moveSpeed;
        moveSpeed = 0;
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(duration);
        moveSpeed = originalSpeed;
    }

    // Efecto Fuego: daño prolongado
    protected IEnumerator BurnEffect(int damagePerTick, float interval)
    {
        for (int i = 0; i < 2; i++)
        {
            yield return new WaitForSeconds(interval);
            currentHealth -= damagePerTick;
            BossHealthUI.Instance?.UpdateHealth(currentHealth, maxHealth);

            // Feedback visual por cada quemadura
            StartCoroutine(ChangeColorTemporarily(Color.red, 0.2f));

            UpdateAfterEffect();
        }
    }

    private void UpdateAfterEffect()
    {
        UpdateAnimator();
        if (currentHealth <= 0)
            Die();
    }

    // Feedback visual: cambio de color temporal
    protected IEnumerator ChangeColorTemporarily(Color color, float duration)
    {
        if (enemySprite == null)
            yield break;

        // Si ya hay una corrutina de color corriendo, la cancelamos
        if (colorCoroutine != null)
        {
            StopCoroutine(colorCoroutine);
            enemySprite.color = baseColor; // restaurar antes de iniciar nuevo cambio
        }

        colorCoroutine = StartCoroutine(ColorChangeRoutine(color, duration));
    }

    private IEnumerator ColorChangeRoutine(Color color, float duration)
    {
        enemySprite.color = color;
        yield return new WaitForSeconds(duration);
        enemySprite.color = baseColor;
        colorCoroutine = null;
    }
}
