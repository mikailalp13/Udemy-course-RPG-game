using UnityEngine;

public class UI_Quest : MonoBehaviour, ISaveable
{
    private GameData current_game_data;

    [SerializeField] private UI_ItemSlotParent inventory_slots;
    [SerializeField] private UI_QuestPreview quest_preview;
    private UI_QuestSlot[] quest_slots;
    public Player_QuestManager quest_manager { get; private set; }


    private void Awake()
    {
        quest_slots = GetComponentsInChildren<UI_QuestSlot>(true);
        quest_manager = Player.instance.quest_manager;
    }


    public void SetupQuestUI(QuestDataSO[] quests_to_setup)
    {
        foreach (var slot in quest_slots)
            slot.gameObject.SetActive(false);

        for (int i = 0; i < quests_to_setup.Length; i++)
        {
            quest_slots[i].gameObject.SetActive(true);
            quest_slots[i].SetupQuestSlot(quests_to_setup[i]);
        }

        quest_preview.MakeQuestPreviewEmpty();
        inventory_slots.UpdateSlots(Player.instance.inventory.item_list);

        UpdateQuestList();
    }


    public void UpdateQuestList()
    {
        foreach (var slot in quest_slots)
        {
            if (slot.quest_in_slot == null)
                continue;
            
            if (slot.gameObject.activeSelf && CanTakeQuest(slot.quest_in_slot) == false)
                slot.gameObject.SetActive(false);
        }
    }


    private bool CanTakeQuest(QuestDataSO quest_to_check)
    {
        bool quest_active = quest_manager.QuestIsActive(quest_to_check);

        if (current_game_data != null)
        {
            bool quest_completed = 
                current_game_data.completed_quests.TryGetValue(quest_to_check.quest_save_id, out bool is_completed) && is_completed;

            return quest_active == false && quest_completed == false;
        }

        return quest_active == false;
    }


    public UI_QuestPreview GetQuestPreview() => quest_preview;

    public void LoadData(GameData data)
    {
        current_game_data = data;
    }

    public void SaveData(ref GameData data)
    {
        
    }
}
