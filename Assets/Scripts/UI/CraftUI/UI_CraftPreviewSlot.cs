using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftPreviewSlot : MonoBehaviour
{
    [SerializeField] private Image material_icon;
    [SerializeField] private TextMeshProUGUI material_name_and_value;

    public void SetupPreviewSlot(ItemDataSO item_data, int avaliable_amount, int required_amount)
    {
        material_icon.sprite = item_data.item_icon;
        material_name_and_value.text = item_data.item_name + " - " + avaliable_amount + "/" + required_amount;
    }
}
