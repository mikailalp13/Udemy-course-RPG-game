using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup / Item Data / Item List", fileName = "List of Items - ")]

public class ItemListDataSO : ScriptableObject
{
    public ItemDataSO[] item_list;



    public ItemDataSO GetItemData(string save_id)
    {
        return item_list.FirstOrDefault(item => item != null && item.save_id == save_id);
    }



#if UNITY_EDITOR
    [ContextMenu("Auto-fill with all ItemDataSO")]
    public void CollectItemsData()
    {
        string[] guids = AssetDatabase.FindAssets("t:ItemDataSO");

        item_list = guids
            .Select(guid => AssetDatabase.LoadAssetAtPath<ItemDataSO>(AssetDatabase.GUIDToAssetPath(guid)))
            .Where(item => item != null)
            .ToArray();

        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }
#endif
}
