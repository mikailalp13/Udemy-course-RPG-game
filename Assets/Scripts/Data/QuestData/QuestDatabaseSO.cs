using UnityEngine;
using UnityEditor;
using System.Linq;

[CreateAssetMenu(menuName = "RPG Setup / Quest Data / Quest Database", fileName = "QUEST DATABASE")]

public class QuestDatabaseSO : ScriptableObject
{
    public QuestDataSO[] all_quests;



    public QuestDataSO GetQuestById(string id)
    {
        return all_quests.FirstOrDefault(q => q != null && q.quest_save_id == id);
    }


#if UNITY_EDITOR
    [ContextMenu("Auto-fill with all QuestDataSO")]
    public void CollectItemsData()
    {
        string[] guids = AssetDatabase.FindAssets("t:QuestDataSO");

        all_quests = guids
            .Select(guid => AssetDatabase.LoadAssetAtPath<QuestDataSO>(AssetDatabase.GUIDToAssetPath(guid)))
            .Where(q => q != null)
            .ToArray();

        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }
#endif
}
