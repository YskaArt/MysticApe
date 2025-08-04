using UnityEngine;

public class IcePillar : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
            if (collision.TryGetComponent<PlayerHealth>(out var playerHealth))
            {
                playerHealth.TakeDamage(damage);
            }
        
    }
}
