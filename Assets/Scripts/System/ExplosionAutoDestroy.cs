using UnityEngine;

public class ExplosionAutoDestroy : MonoBehaviour
{
    // Esta función será llamada desde el evento de animación
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
