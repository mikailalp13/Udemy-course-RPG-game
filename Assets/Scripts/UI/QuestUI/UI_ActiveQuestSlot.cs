using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ActiveQuestSlot : MonoBehaviour
{
    private QuestData quest_in_slot;
    private UI_ActiveQuestPreview quest_preview;

    [SerializeField] private TextMeshProUGUI quest_name;
    [SerializeField] private Image[] quest_reward_preview;



    public void SetupActiveQuestSlot(QuestData quest_to_setup)
    {
        quest_preview = transform.root.GetComponentInChildren<UI_ActiveQuestPreview>();
        quest_in_slot = quest_to_setup;

        quest_name.text = quest_to_setup.quest_dataSO.quest_name;


        Inventory_Item[] reward = quest_to_setup.quest_dataSO.reward_items;


        foreach (var preview_icon in quest_reward_preview)
        {
            preview_icon.gameObject.SetActive(false);
        }

        for (int i = 0; i < reward.Length; i++)
        {
            if (reward[i] == null)
                continue;
            
            Image preview = quest_reward_preview[i];

            preview.gameObject.SetActive(true);
            preview.sprite = reward[i].item_data.item_icon;
            preview.GetComponentInChildren<TextMeshProUGUI>().text = reward[i].stack_size.ToString();
        }
    }


    public void SetupPreviewButton()
    {
        quest_preview.SetupQuestPreview(quest_in_slot);
    }
}
