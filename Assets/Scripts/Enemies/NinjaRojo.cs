using UnityEngine;

public class NinjaRojo : EnemyBase
{
    [Header("Ataque")]
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 1.5f;

    [Header("Armas Direccionales")]
    [SerializeField] private GameObject weaponUp;
    [SerializeField] private GameObject weaponDown;
    [SerializeField] private GameObject weaponLeft;
    [SerializeField] private GameObject weaponRight;

    private GameObject activeWeapon;
    private float cooldownTimer = 0f;

    protected override void UpdateState()
    {
        if (player == null || currentState == EnemyState.Dead)
            return;

        float distance = Vector2.Distance(player.position, transform.position);
        Vector2 dirToPlayer = (player.position - transform.position).normalized;

        if (dirToPlayer != Vector2.zero)
            lastMoveDir = dirToPlayer;

        //  Fase de recuperación después del ataque
        if (cooldownTimer > 0f)
        {
            currentState = EnemyState.Idle;
            rb.linearVelocity = Vector2.zero;
            moveDirection = Vector2.zero;
            cooldownTimer -= Time.deltaTime;
            return;
        }

        //  Ataque si está en rango
        if (distance <= attackRange)
        {
            
            StartAttack(dirToPlayer);
            currentState = EnemyState.Attack;
           

            
            cooldownTimer = attackCooldown;
        }
        //  Si está fuera de rango: perseguir
        else
        {
            currentState = EnemyState.Chase;
            moveDirection = dirToPlayer;
            rb.linearVelocity = dirToPlayer * moveSpeed;
        }
    }

    private void StartAttack(Vector2 dir)
    {
        animator.SetTrigger("Attack");

        DisableAllWeapons();

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            activeWeapon = dir.x > 0 ? weaponRight : weaponLeft;
        else
            activeWeapon = dir.y > 0 ? weaponUp : weaponDown;

        activeWeapon.SetActive(true);
        Invoke(nameof(EndAttack), 0.3f); // Sincronizado con la animación
    }

    private void EndAttack()
    {
        if (activeWeapon != null)
            activeWeapon.SetActive(false);
        rb.linearVelocity = Vector2.zero;
        moveDirection = Vector2.zero;
    }

    private void DisableAllWeapons()
    {
        weaponUp.SetActive(false);
        weaponDown.SetActive(false);
        weaponLeft.SetActive(false);
        weaponRight.SetActive(false);
    }
}
