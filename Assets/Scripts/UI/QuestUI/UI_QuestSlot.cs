using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_QuestSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI quest_name;
    [SerializeField] private Image[] reward_quick_preview_slots;

    public QuestDataSO quest_in_slot { get; private set; }
    private UI_QuestPreview quest_preview;



    public void SetupQuestSlot(QuestDataSO quest_dataSO)
    {
        quest_preview = transform.root.GetComponentInChildren<UI_Quest>().GetQuestPreview();
        quest_in_slot = quest_dataSO;
        quest_name.text = quest_dataSO.quest_name;

        foreach (var preview_icon in reward_quick_preview_slots)
        {
            preview_icon.gameObject.SetActive(false);
        }

        for (int i = 0; i < quest_in_slot.reward_items.Length; i++)
        {
            if (quest_dataSO.reward_items[i] == null || quest_dataSO.reward_items[i].item_data == null)
                continue;

            Image slot = reward_quick_preview_slots[i];

            slot.gameObject.SetActive(true);
            slot.sprite = quest_dataSO.reward_items[i].item_data.item_icon;
            slot.GetComponentInChildren<TextMeshProUGUI>().text = quest_dataSO.reward_items[i].stack_size.ToString();
        }
    }


    public void UpdateQuestPreview()
    {
        quest_preview.SetupQuestPreview(quest_in_slot);
    }
}
