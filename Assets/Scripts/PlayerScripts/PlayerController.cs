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

    [SerializeField] private UIAbilitySelector abilityUI;
    [SerializeField] private float interactRange = 1f;
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueActive)
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("IsMoving", false);
            return; // evita movimiento y acciones
        }
        HandleMovementInput();
        HandleElementChange();
        HandleAttack();
        RotateFirePoint();
        HandleInteraction();

    }

    private void FixedUpdate()
    {
        if (DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueActive)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
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
        int index = (int)currentElement;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!abilityUI.CanUseAbility(index)) return; // cooldown activo

            animator.SetTrigger("Attack");

            Transform firePoint = GetFirePointByDirection(lastMoveDir);

            GameObject projectile = Instantiate(
                abilityProjectiles[index],
                firePoint.position,
                Quaternion.identity
            );

            projectile.GetComponent<ProjectileBase>().SetDirection(lastMoveDir);

            abilityUI.TriggerCooldown(index);
        }
    }

    private Transform GetFirePointByDirection(Vector2 dir)
    {
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            return dir.x > 0 ? firePointRight : firePointLeft;
        else
            return dir.y > 0 ? firePointUp : firePointDown;
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
    private void HandleInteraction()
    {
        if (Input.GetKeyDown(interactKey))
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactRange);
            foreach (var hit in hits)
            {
                IInteractable interactable = hit.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.Interact();
                    break;
                }
            }
        }
    }

}
