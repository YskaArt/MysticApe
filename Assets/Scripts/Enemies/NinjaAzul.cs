using UnityEngine;

public class NinjaAzul : EnemyBase
{
    [SerializeField] private float shootCooldown = 2f;
    [SerializeField] private float shootRange = 8f;
    [SerializeField] private GameObject kunaiPrefab;

    [SerializeField] private Transform firePointUp;
    [SerializeField] private Transform firePointDown;
    [SerializeField] private Transform firePointLeft;
    [SerializeField] private Transform firePointRight;

    private float cooldownTimer;

    protected override void UpdateState()
    {
        if (player == null) return;

        float distance = Vector2.Distance(player.position, transform.position);

        if (distance <= shootRange)
        {
            currentState = EnemyState.Attack;

            Vector2 dir = (player.position - transform.position).normalized;
            lastMoveDir = dir;
            rb.linearVelocity = Vector2.zero;

            if (cooldownTimer <= 0f)
            {
                animator.SetTrigger("Attack");

                Transform selectedFirePoint = GetFirePointFromDirection(dir);
                GameObject proj = Instantiate(kunaiPrefab, selectedFirePoint.position, Quaternion.identity);
                proj.GetComponent<EnemyProjectile>().SetDirection(dir);

                cooldownTimer = shootCooldown;
            }
        }
        else
        {
            currentState = EnemyState.Idle;
            rb.linearVelocity = Vector2.zero;
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
}
