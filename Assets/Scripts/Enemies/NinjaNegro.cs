using UnityEngine;

public class NinjaNegro : EnemyBase
{
    [Header("Ataque")]
    [SerializeField] private float shootCooldown = 3f;
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private GameObject bombPrefab;

    [Header("FirePoints")]
    [SerializeField] private Transform firePointUp;
    [SerializeField] private Transform firePointDown;
    [SerializeField] private Transform firePointLeft;
    [SerializeField] private Transform firePointRight;

    private float cooldownTimer;

    protected override void UpdateState()
    {
        if (player == null || currentState == EnemyState.Dead) return;

        float distance = Vector2.Distance(player.position, transform.position);
        Vector2 dirToPlayer = (player.position - transform.position).normalized;

        if (dirToPlayer != Vector2.zero)
            lastMoveDir = dirToPlayer;

        if (distance > attackRange)
        {
            currentState = EnemyState.Chase;
            moveDirection = dirToPlayer;
            rb.linearVelocity = dirToPlayer * moveSpeed;
        }
        else
        {
            currentState = EnemyState.Attack;
            rb.linearVelocity = Vector2.zero;
            moveDirection = Vector2.zero;

            if (cooldownTimer <= 0f)
            {
                animator.SetTrigger("Attack");

                Transform selectedFirePoint = GetFirePointFromDirection(dirToPlayer);
                LaunchBomb(selectedFirePoint, player.position);

                cooldownTimer = shootCooldown;
            }
        }

        cooldownTimer -= Time.deltaTime;
    }


    private Transform GetFirePointFromDirection(Vector2 dir)
    {
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            return dir.x > 0 ? firePointRight : firePointLeft;
        else
            return dir.y > 0 ? firePointUp : firePointDown;
    }

    private void LaunchBomb(Transform firePoint, Vector2 targetPosition)
    {
        GameObject bomb = Instantiate(bombPrefab, firePoint.position, Quaternion.identity);
        bomb.GetComponent<SmartBomb>()?.SetTarget(targetPosition);
    }
}
