
using UnityEngine;
public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private InventoryUIManager inventoryUI;
    [SerializeField] private InventorySystem inventorySystem;

    private bool isPaused = false;

    public static PauseMenuManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ForceClosePause()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!DialogueManager.Instance.IsDialogueActive)
            {
                TogglePause();
            }
            
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pausePanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1;

        if (!isPaused)
            inventoryUI.CloseInventory();
    }

    public void OpenInventory()
    {
        inventoryUI.OpenInventory(inventorySystem.GetItems());
    }

    public void QuitToMainMenu()
    {
        Debug.Log("volviendo al Main Menu");
        // To do...
    }
}
