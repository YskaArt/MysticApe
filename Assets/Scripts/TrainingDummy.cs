using UnityEngine;

public class TrainingDummy : MonoBehaviour
{
    [SerializeField] private PlayerController.Element requiredElement;
    private bool destroyed = false;

    public delegate void DummyDestroyed();
    public static event DummyDestroyed OnDummyDestroyed;

    public void Hit(PlayerController.Element element)
    {
        if (destroyed) return;

        if (element == requiredElement)
        {
            destroyed = true;
            OnDummyDestroyed?.Invoke(); // Avisar al sistema de Quest
            Destroy(gameObject); 
        }
    }
}
