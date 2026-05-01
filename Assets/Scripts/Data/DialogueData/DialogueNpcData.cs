using System;
using UnityEngine;

[Serializable]
public class DialogueNpcData 
{
    public RewardType npc_reward_type;
    public QuestDataSO[] quests;


    public DialogueNpcData(RewardType npc_reward_type, QuestDataSO[] quests)
    {
        this.npc_reward_type = npc_reward_type;
        this.quests = quests;
    }
}
