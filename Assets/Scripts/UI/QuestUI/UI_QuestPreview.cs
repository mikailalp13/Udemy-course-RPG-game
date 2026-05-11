using TMPro;
using UnityEngine;

public class UI_QuestPreview : MonoBehaviour
{
    private UI_Quest quest_ui;
    private QuestDataSO preview_quest;

    [SerializeField] private TextMeshProUGUI quest_name;
    [SerializeField] private TextMeshProUGUI quest_goal;
    [SerializeField] private TextMeshProUGUI quest_description;
    [SerializeField] private GameObject[] additional_objects;
    [SerializeField] private UI_QuestRewardSlot[] quest_reward;


    public void SetupQuestPreview(QuestDataSO quest_dataSO)
    {
        quest_ui = transform.root.GetComponentInChildren<UI_Quest>();
        preview_quest = quest_dataSO;

        EnableAdditionalObjects(true);   
        EnableQuestRewardObjects(true);

        quest_name.text = quest_dataSO.quest_name;
        quest_description.text = quest_dataSO.description;
        quest_goal.text = quest_dataSO.quest_goal + " " + quest_dataSO.required_amount;

        foreach (var obj in quest_reward)
            obj.gameObject.SetActive(false);

        for (int i = 0; i < quest_dataSO.reward_items.Length; i++)
        {
            Inventory_Item reward_item = new Inventory_Item(quest_dataSO.reward_items[i].item_data);
            reward_item.stack_size = quest_dataSO.reward_items[i].stack_size;

            quest_reward[i].gameObject.SetActive(true);
            quest_reward[i].UpdateSlot(reward_item);
        }
    }


    public void AcceptQuestButton()
    {
        MakeQuestPreviewEmpty();

        quest_ui.quest_manager.AcceptQuest(preview_quest);
        quest_ui.UpdateQuestList();
    }


    public void MakeQuestPreviewEmpty()
    {
        quest_name.text = "";
        quest_description.text = "";

        EnableAdditionalObjects(false);
        EnableQuestRewardObjects(false);
    }


    private void EnableAdditionalObjects(bool enable)
    {
        foreach (var obj in additional_objects)
            obj.SetActive(enable);
    }


    private void EnableQuestRewardObjects(bool enable)
    {
        foreach (var obj in quest_reward)
            obj.gameObject.SetActive(enable);
    }
}
