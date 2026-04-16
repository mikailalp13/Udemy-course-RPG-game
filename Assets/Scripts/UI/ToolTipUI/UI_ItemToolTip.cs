using TMPro;
using UnityEngine;
using System.Text;

public class UI_ItemToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI item_name;
    [SerializeField] private TextMeshProUGUI item_type;
    [SerializeField] private TextMeshProUGUI item_info;

    [SerializeField] private TextMeshProUGUI item_price;
    [SerializeField] private Transform merchant_info;
    [SerializeField] private Transform inventory_info;


    public void ShowToolTip(bool show, RectTransform target_rect, Inventory_Item item_to_show, bool buy_price = false, bool show_merchant_info = false)
    {
        base.ShowToolTip(show, target_rect);

        merchant_info.gameObject.SetActive(show_merchant_info);
        inventory_info.gameObject.SetActive(!show_merchant_info);

        int price = buy_price ? item_to_show.buy_price : Mathf.FloorToInt(item_to_show.sell_price);
        int total_price = price * item_to_show.stack_size;

        string full_stack_price = ($"Price: {price} x {item_to_show.stack_size} - {total_price}g.");
        string single_stack_price = ($"Price: {price}g.");

        item_info.text = item_to_show.GetItemInfo();
        item_type.text = item_to_show.item_data.item_type.ToString();
        item_price.text = item_to_show.stack_size > 1 ? full_stack_price : single_stack_price;

        string color = GetColorByRarity(item_to_show.item_data.item_rarity);
        item_name.text = GetColoredText(color, item_to_show.item_data.item_name);;
    }

    private string GetColorByRarity(int rarity)
    {
        if (rarity <= 100) return "white";  // common
        if (rarity <= 300) return "green";  // uncommon
        if (rarity <= 600) return "blue";   // rare
        if (rarity <= 850) return "purple"; // epic
        return "orange";                    // legendary
    }
}
