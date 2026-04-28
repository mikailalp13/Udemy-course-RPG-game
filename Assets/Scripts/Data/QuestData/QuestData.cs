using System;
using UnityEngine;

[Serializable]
public class QuestData
{
    public QuestDataSO quest_dataSO;
    public int current_amount;
    public bool can_get_reward;



    public void AddQuestProgress(int amount = 1)
    {
        current_amount += amount;
        can_get_reward = CanGetReward();
    }


    public bool CanGetReward() => current_amount >= quest_dataSO.required_amount;

    public QuestData(QuestDataSO quest_dataSO)
    {
        this.quest_dataSO = quest_dataSO;

    }
}
