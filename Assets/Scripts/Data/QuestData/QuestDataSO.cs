using UnityEditor;
using UnityEngine;

public enum RewardType { Merchant, Blacksmith, None };
public enum QuestType { Kill, Talk, Delivery };


[CreateAssetMenu(menuName = "RPG Setup / Quest Data / New Quest", fileName = "Quest - ")]
public class QuestDataSO : ScriptableObject
{
    public string quest_save_id;
    

    [Space]
    public QuestType quest_type;
    public string quest_name;


    [TextArea] public string description;
    [TextArea] public string quest_goal;


    public string quest_target_id; // enemy name, item name, NPC name etc. the thing to get to complete the quest
    public int required_amount;
    public ItemDataSO item_to_deliver; // Used only if quest type is delivery


    [Header("Reward")]
    public RewardType reward_type;
    public Inventory_Item[] reward_items;
    


    private void OnValidate()
    {
#if UNITY_EDITOR
        string path = AssetDatabase.GetAssetPath(this);
        quest_save_id = AssetDatabase.AssetPathToGUID(path);
#endif
    }
}
