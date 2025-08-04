using UnityEngine;
using System.Collections;

public class ShogunBoss : EnemyBase
{
    [Header("Ataques")]
    [SerializeField] private GameObject iceProjectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;

    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private int minBombs = 4;
    [SerializeField] private int maxBombs = 8;
    [SerializeField] private float bombRadius = 3f;

    [SerializeField] private GameObject icePillarPrefab;
    [SerializeField] private int numberOfPillars = 6;
    [SerializeField] private float pillarSpawnRadius = 4f;
    [SerializeField] private float pillarTriggerDistance = 1.2f;

    [Header("Daño frontal")]
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private LayerMask playerLayer;

    [Header("IA")]
    [SerializeField] private float attackDistance = 1.5f;
    [SerializeField] private float chaseDistance = 6f;
    [SerializeField] private float minIdleAttackTime = 3f;
    [SerializeField] private float maxIdleAttackTime = 6f;

    private bool isAttacking = false;
    private float timeSinceLastAttack;
    private float nextRandomAttackTime;
    public event System.Action OnBossDefeated;
    
    protected override void Start()
    {
        base.Start();

        if (BossHealthUI.Instance != null)
        {
            BossHealthUI.Instance.SetMaxHealth(maxHealth); 
        }
    }
    protected override void UpdateState()
    {
        if (currentState == EnemyState.Dead || player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (isAttacking)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (distanceToPlayer <= attackDistance)
        {
            rb.linearVelocity = Vector2.zero;
            isAttacking = true;
            timeSinceLastAttack = 0f;

           
            if (distanceToPlayer <= pillarTriggerDistance)
            {
                animator.SetTrigger("ChargeLeft");
                StartCoroutine(WaitAfterSpecialAttack(1.2f));
            }
            else
            {
                StartCoroutine(PerformRandomAttack());
            }
        }
        else if (distanceToPlayer <= chaseDistance)
        {
            moveDirection = (player.position - transform.position).normalized;
            rb.linearVelocity = moveDirection * moveSpeed;

            timeSinceLastAttack += Time.deltaTime;
            if (timeSinceLastAttack >= nextRandomAttackTime)
            {
                timeSinceLastAttack = 0f;
                nextRandomAttackTime = Random.Range(minIdleAttackTime, maxIdleAttackTime);
                isAttacking = true;
                StartCoroutine(PerformRandomAttack());
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    protected override void UpdateAnimator()
    {
        animator.SetBool("IsWalking", rb.linearVelocity.magnitude > 0.1f);
    }

    public void PerformFrontalAttack()
    {
        Vector2 attackPoint = (Vector2)transform.position + Vector2.down * attackRange;
        Collider2D hit = Physics2D.OverlapCircle(attackPoint, 2f, playerLayer);

        if (hit != null && hit.TryGetComponent(out PlayerHealth playerHealth))
        {
            playerHealth.TakeDamage(1);
            
        }
    }

    public void ShootIceProjectiles()
    {
        if (player == null || iceProjectilePrefab == null) return;

        Vector2 direction = (player.position - projectileSpawnPoint.position).normalized;

        // Disparo directo
        GameObject centerProj = Instantiate(iceProjectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        if (centerProj.TryGetComponent<EnemyProjectile>(out var centerProjectile))
        {
            centerProjectile.SetDirection(direction);
        }

        // Disparos con spread
        for (int i = -1; i <= 1; i += 2)
        {
            Vector2 spread = Quaternion.Euler(0, 0, i * 10f) * direction;
            GameObject proj = Instantiate(iceProjectilePrefab, projectileSpawnPoint.position, Quaternion.identity);

            if (proj.TryGetComponent<EnemyProjectile>(out var projectile))
            {
                projectile.SetDirection(spread);
            }
        }
    }

    public void ThrowBombsAroundPlayer()
    {
        if (bombPrefab == null || player == null) return;

        int bombCount = Random.Range(minBombs, maxBombs + 1);

        for (int i = 0; i < bombCount; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle.normalized * Random.Range(1f, bombRadius);
            Vector2 spawnPos = (Vector2)player.position + randomOffset;

            GameObject bomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);

            if (bomb.TryGetComponent<SmartBomb>(out var smartBomb))
            {
                smartBomb.SetTarget(spawnPos);
            }
        }
    }

    public void SpawnIcePillars()
    {
        for (int i = 0; i < numberOfPillars; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle.normalized * Random.Range(1f, pillarSpawnRadius);
            Vector2 spawnPos = (Vector2)transform.position + randomOffset;

            Instantiate(icePillarPrefab, spawnPos, Quaternion.identity);
        }
    }

    protected override void Die()
    {
        base.Die();
        animator.SetTrigger("IsDeath");
        OnBossDefeated?.Invoke();
        Debug.Log("Shogun derrotado.");
    }

    private IEnumerator PerformRandomAttack()
    {
        int random = Random.Range(0, 4);
        switch (random)
        {
            case 0: animator.SetTrigger("AttackLeft"); break;
            case 1: animator.SetTrigger("AttackRight"); break;
            case 2: animator.SetTrigger("ChargeLeft"); break;
            case 3: animator.SetTrigger("ChargeRight"); break;
        }

        yield return new WaitForSeconds(1f);
        isAttacking = false;
    }

    private IEnumerator WaitAfterSpecialAttack(float delay)
    {
        yield return new WaitForSeconds(delay);
        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 center = (Vector2)transform.position + lastMoveDir.normalized * attackRange;
        Gizmos.DrawWireSphere(center, 2f);
    }
    public override void TakeDamage(int damage, PlayerController.Element sourceElement)
    {
        base.TakeDamage(damage, sourceElement); 

        if (BossHealthUI.Instance != null)
        {
            BossHealthUI.Instance.UpdateHealth(currentHealth, maxHealth);
        }
    }
}
