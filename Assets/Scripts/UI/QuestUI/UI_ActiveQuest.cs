using System.Collections.Generic;
using UnityEngine;

public class UI_ActiveQuest : MonoBehaviour
{
    private Player_QuestManager quest_manager;
    private UI_ActiveQuestSlot[] quest_slots;


    private void Awake()
    {
        quest_manager = Player.instance.quest_manager;
        quest_slots = GetComponentsInChildren<UI_ActiveQuestSlot>(true);
    }


    private void OnEnable()
    {
        List<QuestData> quests = quest_manager.active_quests;

        foreach (var slot in quest_slots)
        {
            slot.gameObject.SetActive(false);
        }

        for (int i = 0; i < quests.Count; i++)
        {
            quest_slots[i].gameObject.SetActive(true);
            quest_slots[i].SetupActiveQuestSlot(quests[i]);
        }

        if (quests.Count > 0)
            quest_slots[0].SetupPreviewButton();
    }
}
