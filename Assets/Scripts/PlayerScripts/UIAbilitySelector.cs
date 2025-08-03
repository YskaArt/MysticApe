using UnityEngine;
using UnityEngine.UI;

public class UIAbilitySelector : MonoBehaviour
{
    [SerializeField] private RectTransform selector; // El marco que se mueve
    [SerializeField] private Image[] abilityIcons;   // Iconos de cada habilidad
    [SerializeField] private Image[] cooldownOverlays; // Imagen encima que se llena por cooldown

    private float[] cooldownTimers;
    [SerializeField] private float cooldownDuration = 1.5f;

    private int currentIndex = 0;

    private void Start()
    {
        cooldownTimers = new float[4];
        UpdateSelectorPosition();
    }

    private void Update()
    {
        HandleInput();
        UpdateCooldownUI();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SetActiveAbility(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SetActiveAbility(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SetActiveAbility(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SetActiveAbility(3);
    }

    public void SetActiveAbility(int index)
    {
        currentIndex = index;
        UpdateSelectorPosition();
    }

    private void UpdateSelectorPosition()
    {
        selector.position = abilityIcons[currentIndex].transform.position;
    }

    private void UpdateCooldownUI()
    {
        for (int i = 0; i < cooldownTimers.Length; i++)
        {
            if (cooldownTimers[i] > 0)
            {
                cooldownTimers[i] -= Time.deltaTime;
                cooldownOverlays[i].fillAmount = cooldownTimers[i] / cooldownDuration;
            }
            else
            {
                cooldownOverlays[i].fillAmount = 0;
            }
        }
    }

    public bool CanUseAbility(int index)
    {
        return cooldownTimers[index] <= 0;
    }

    public void TriggerCooldown(int index)
    {
        cooldownTimers[index] = cooldownDuration;
    }

    public int GetCurrentIndex()
    {
        return currentIndex;
    }
}
