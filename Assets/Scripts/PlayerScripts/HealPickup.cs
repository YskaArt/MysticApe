using UnityEngine;

public class HealPickup : MonoBehaviour
{
    [SerializeField] private int healAmount = 4; // cura 1 corazón completo (4 puntos)
    //[SerializeField] private AudioClip pickupSFX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.Heal(healAmount);

                //if (pickupSFX != null)
                    //AudioSource.PlayClipAtPoint(pickupSFX, transform.position); Para sonido de HEAL

                Destroy(gameObject); // destruir el PowerUp
            }
        }
    }
}
