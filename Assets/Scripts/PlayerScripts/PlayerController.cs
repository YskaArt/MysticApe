using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 lastMoveDir = Vector2.down;
    private Animator animator;
    public enum Element
    {
        Planta, Roca, Fuego, Hielo
    }
    [Header("Disparo Elemental")]
    [SerializeField] private Element currentElement = Element.Planta;
    [SerializeField] private GameObject[] abilityProjectiles; // orden: 0-Planta, 1-Roca, 2-Fuego, 3-Hielo
    [SerializeField] private Transform firePointUp;
    [SerializeField] private Transform firePointDown;
    [SerializeField] private Transform firePointLeft;
    [SerializeField] private Transform firePointRight;
    private Transform selectedFirePoint;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        
        HandleMovementInput();
        HandleElementChange();
        HandleAttack();
        RotateFirePoint();
        
    }

    private void FixedUpdate()
    {
        UpdateAnimatorParameters();
        rb.linearVelocity = moveInput * moveSpeed;
        selectedFirePoint = firePointDown;
    }

    private void HandleMovementInput()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        if (moveInput != Vector2.zero)
        {
            lastMoveDir = moveInput.normalized;
        }
        moveInput.Normalize();
    }

    private void HandleElementChange()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) currentElement = Element.Planta;
        if (Input.GetKeyDown(KeyCode.Alpha2)) currentElement = Element.Roca;
        if (Input.GetKeyDown(KeyCode.Alpha3)) currentElement = Element.Fuego;
        if (Input.GetKeyDown(KeyCode.Alpha4)) currentElement = Element.Hielo;
    }

    private void HandleAttack()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            animator.SetTrigger("Attack");
            int index = (int)currentElement;

            Transform selectedFirePoint = firePointDown; // por defecto

            if (Mathf.Abs(lastMoveDir.x) > Mathf.Abs(lastMoveDir.y))
            {
                selectedFirePoint = lastMoveDir.x > 0 ? firePointRight : firePointLeft;
            }
            else
            {
                selectedFirePoint = lastMoveDir.y > 0 ? firePointUp : firePointDown;
            }

            GameObject projectile = Instantiate(
                abilityProjectiles[index],
                selectedFirePoint.position,
                Quaternion.identity
            );

            projectile.GetComponent<ProjectileBase>().SetDirection(lastMoveDir);

            projectile.GetComponent<ProjectileBase>().SetDirection(lastMoveDir);
        }
    }

    private void RotateFirePoint()
    {
        if (lastMoveDir != Vector2.zero)
        {
            float angle = Mathf.Atan2(lastMoveDir.y, lastMoveDir.x) * Mathf.Rad2Deg;
            selectedFirePoint.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
    private void UpdateAnimatorParameters()
    {
        animator.SetFloat("MoveX", moveInput.x);
        animator.SetFloat("MoveY", moveInput.y);

        animator.SetBool("IsMoving", moveInput != Vector2.zero);

        animator.SetFloat("LastMoveX", lastMoveDir.x);
        animator.SetFloat("LastMoveY", lastMoveDir.y);
    }

}
