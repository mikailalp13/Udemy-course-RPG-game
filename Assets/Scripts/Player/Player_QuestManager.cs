using UnityEngine;
using System.Collections.Generic;

public class Player_QuestManager : MonoBehaviour, ISaveable
{
    public List<QuestData> active_quests;
    public List<QuestData> completed_quests;

    private Entity_DropManager drop_manager;
    private Inventory_Player inventory;


    [Header("Quest Database")]
    [SerializeField] private QuestDatabaseSO quest_database;


    private void Awake()
    {
        drop_manager = GetComponent<Entity_DropManager>();
        inventory = GetComponent<Inventory_Player>();     
    }


    public void TryGiveRewardFrom(RewardType npc_type)
    {
        List<QuestData> get_reward_quests = new List<QuestData>();

        foreach (var quest in active_quests)
        {
            // Deliver items if can
            if (quest.quest_dataSO.quest_type == QuestType.Delivery)
            {
                ItemDataSO required_item = quest.quest_dataSO.item_to_deliver;
                int required_amount = quest.quest_dataSO.required_amount;

                if (inventory.HasItemAmount(required_item, required_amount))
                {
                    inventory.RemoveItemAmount(required_item, required_amount);
                    quest.AddQuestProgress(required_amount);
                }
            }

            if (quest.CanGetReward() && quest.quest_dataSO.reward_type == npc_type)
                get_reward_quests.Add(quest);
        }

        foreach (var quest in get_reward_quests)
        {
            GiveQuestReward(quest.quest_dataSO);
            CompleteQuest(quest);
        }
    }


    private void GiveQuestReward(QuestDataSO quest_dataSO)
    {
        foreach (var item in quest_dataSO.reward_items)
        {
            if (item == null || item.item_data == null)
                continue;
            
            for (int i = 0; i < item.stack_size; i++)
            {
                drop_manager.CreateItemDrop(item.item_data);
            }
        }
    }


    public void AddProgress(string quest_target_id, int amount = 1)
    {
        List<QuestData> get_reward_quests = new List<QuestData>();

        foreach (var quest in active_quests)
        {
            if (quest.quest_dataSO.quest_target_id != quest_target_id)
                continue;
            
            if (quest.CanGetReward() == false)
                quest.AddQuestProgress(amount);

            if (quest.quest_dataSO.reward_type == RewardType.None && quest.CanGetReward())
                get_reward_quests.Add(quest);
        }

        foreach (var quest in get_reward_quests)
        {
            GiveQuestReward(quest.quest_dataSO);
            CompleteQuest(quest);
        }
    }


    public int GetQuestProgress(QuestData quest_to_check)
    {
        QuestData quest = active_quests.Find(q => q == quest_to_check);

        return quest != null ? quest.current_amount : 0;
    }


    public void AcceptQuest(QuestDataSO quest_dataSO)
    {
        active_quests.Add(new QuestData(quest_dataSO));
    }


    public void CompleteQuest(QuestData quest_data)
    {
        completed_quests.Add(quest_data);
        active_quests.Remove(quest_data);
    }


    public bool QuestIsActive(QuestDataSO quest_to_check)
    {
        if (quest_to_check == null)
            return false;
        
        return active_quests.Find(q => q.quest_dataSO == quest_to_check) != null;
    }

    public void LoadData(GameData data)
    {
        active_quests.Clear();

        foreach (var entry in data.active_quests)
        {
            string quest_save_id = entry.Key;
            int progress = entry.Value;

            QuestDataSO quest_dataSO = quest_database.GetQuestById(quest_save_id);

            if (quest_dataSO == null)
            {
                Debug.Log(quest_save_id + "is not found in the database!");
                continue;
            }

            QuestData quest_to_load = new QuestData(quest_dataSO);
            quest_to_load.current_amount = progress;

            active_quests.Add(quest_to_load);
        }
    }

    public void SaveData(ref GameData data)
    {
        data.active_quests.Clear();

        foreach (var quest in active_quests)
        {
            data.active_quests.Add(quest.quest_dataSO.quest_save_id, quest.current_amount);
        }

        foreach (var quest in completed_quests)
        {
            data.completed_quests.Add(quest.quest_dataSO.quest_save_id, true);
        }
    }
}