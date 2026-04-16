using UnityEngine;

public class UI_CraftListButton : MonoBehaviour
{
    [SerializeField] private ItemListDataSO craft_data;
    private UI_CraftSlot[] craft_slots;


    public void SetCraftSlots(UI_CraftSlot[] craft_slots) => this.craft_slots = craft_slots;

    public void UpdateCraftSlots()
    {
        if (craft_data == null)
        {
            Debug.Log("You need to assign craft list data!");
            return;
        }
        
        foreach (var slot in craft_slots)
        {
            slot.gameObject.SetActive(false);
        }

        for (int i = 0; i < craft_data.item_list.Length; i++)
        {
            ItemDataSO item_data = craft_data.item_list[i];

            craft_slots[i].gameObject.SetActive(true);
            craft_slots[i].SetupButton(item_data);
        }
    }
}
