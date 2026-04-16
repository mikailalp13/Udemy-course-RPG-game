using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftSlot : MonoBehaviour
{
    private ItemDataSO item_to_craft;
    [SerializeField] private UI_CraftPreview craft_preview;

    [SerializeField] private Image craft_item_icon;
    [SerializeField] private TextMeshProUGUI craft_item_name;


    public void SetupButton(ItemDataSO craft_data)
    {
        this.item_to_craft = craft_data;

        craft_item_icon.sprite = craft_data.item_icon;
        craft_item_name.text = craft_data.item_name;
    }


    public void UpdateCraftPreview() => craft_preview.UpdateCraftPreview(item_to_craft);

}
