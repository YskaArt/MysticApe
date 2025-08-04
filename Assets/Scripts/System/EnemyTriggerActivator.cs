using UnityEngine;

public class EnemyTriggerActivator2D : MonoBehaviour
{
    [SerializeField] private GameObject[] enemiesToActivate;

    [Header("Una sola vez o varias?")]
    [SerializeField] private bool triggerOnce = true;

    private bool alreadyTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (alreadyTriggered && triggerOnce) return;

        if (other.CompareTag("Player"))
        {
            foreach (GameObject enemy in enemiesToActivate)
            {
                if (enemy != null)
                    enemy.SetActive(true); // activa al enemigo (debe estar inicialmente desactivado)
            }

            alreadyTriggered = true;
        }
    }
}
