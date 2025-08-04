using UnityEngine;

public class QuestNPC_Trainer : QuestNPC
{
    [SerializeField] private int totalDummies = 4;
    private int dummiesDestroyed = 0;

    [Header("Diálogos")]
    [TextArea][SerializeField] private string[] initialDialogue;
    [TextArea][SerializeField] private string[] inProgressDialogue;
    [TextArea][SerializeField] private string[] completionDialogue;

    protected void Start()
    {
        
        TrainingDummy.OnDummyDestroyed += HandleDummyDestroyed;
    }

    private void OnDestroy()
    {
        TrainingDummy.OnDummyDestroyed -= HandleDummyDestroyed;
    }

    private void HandleDummyDestroyed()
    {
        dummiesDestroyed++;

        if (dummiesDestroyed >= totalDummies)
        {
            questCompleted = true;
        }
    }


    public override void Interact()
    {
        InventorySystem inventory = Object.FindFirstObjectByType<InventorySystem>();

        if (!questCompleted)
        {
            if (dummiesDestroyed == 0)
                DialogueManager.Instance.StartDialogue(npcName, npcIcon, initialDialogue);
            else
                DialogueManager.Instance.StartDialogue(npcName, npcIcon, inProgressDialogue);
        }
        else if (!inventory.HasItem(rewardItem))
        {
            inventory.AddItem(rewardItem);
            DialogueManager.Instance.ShowMessage($"¡Excelente trabajo! Has recibido {rewardItem.ItemName}.");
        }
        else
        {
            DialogueManager.Instance.StartDialogue(npcName, npcIcon, completionDialogue);
        }
    }
}
