using TMPro;
using UnityEngine;

public class UI_ActiveQuestPreview : MonoBehaviour
{
    private Player_QuestManager quest_manager;

    [SerializeField] private TextMeshProUGUI quest_name;
    [SerializeField] private TextMeshProUGUI quest_description;
    [SerializeField] private TextMeshProUGUI quest_progress;
    [SerializeField] private UI_QuestRewardSlot[] quest_reward_slots;



    public void SetupQuestPreview(QuestData quest_data)
    {
        quest_manager = Player.instance.quest_manager;
        QuestDataSO questSO = quest_data.quest_dataSO;

        quest_name.text = questSO.name;
        quest_description.text = questSO.description;

        quest_progress.text = questSO.quest_goal + " " + quest_manager.GetQuestProgress(quest_data) + " / " + questSO.required_amount;


        foreach (var obj in quest_reward_slots)
            obj.gameObject.SetActive(false);

        for (int i = 0; i < questSO.reward_items.Length; i++)
        {
            quest_reward_slots[i].gameObject.SetActive(true);
            quest_reward_slots[i].UpdateSlot(questSO.reward_items[i]);
        }
    }
}
