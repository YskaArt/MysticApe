using UnityEngine;

public class ExplosionAutoDestroy : MonoBehaviour
{
    // Esta funci�n ser� llamada desde el evento de animaci�n
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
