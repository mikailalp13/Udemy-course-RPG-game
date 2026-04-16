using UnityEngine;
using System.Collections.Generic;

public class Inventory_Merchant : Inventory_Base
{
    private Inventory_Player inventory;
    [SerializeField] private ItemListDataSO shop_data;
    [SerializeField] private int min_items_amount = 4;


    protected override void Awake()
    {
        base.Awake();
        FillShopList();
    }


    public void TryBuyItem(Inventory_Item item_to_buy, bool buy_full_stack)
    {
        int amount_to_buy = buy_full_stack ? item_to_buy.stack_size : 1;

        for (int i = 0; i < amount_to_buy; i++)
        {
            if (inventory.gold < item_to_buy.buy_price)
            {
                Debug.Log("Not enough gold!");
                return;
            }

            if (item_to_buy.item_data.item_type == ItemType.Material)
            {
                inventory.storage.AddMaterialToStash(item_to_buy);
            }
            else
            {
                if (inventory.CanAddItem(item_to_buy))
                {
                    var item_to_add = new Inventory_Item(item_to_buy.item_data);
                    inventory.AddItem(item_to_add);
                }    
            }

            inventory.gold -= item_to_buy.buy_price;
            RemoveOneItem(item_to_buy);
        }

        TriggerUpdateUI();
    }

    public void TrySellItem(Inventory_Item item_to_sell, bool sell_full_stack)
    {
        int amount_to_sell = sell_full_stack ? item_to_sell.stack_size: 1;

        for (int i = 0; i < amount_to_sell; i++)
        {
            int sell_price = Mathf.FloorToInt(item_to_sell.sell_price);

            inventory.gold += sell_price;
            inventory.RemoveOneItem(item_to_sell);
        }

        TriggerUpdateUI();
    }

    public void FillShopList()
    {
        item_list.Clear();
        List<Inventory_Item> possible_items = new List<Inventory_Item>();

        foreach (var item_data in shop_data.item_list)
        {
            int randomized_stack = Random.Range(item_data.min_stack_size_at_shop, item_data.max_stack_size_at_shop + 1);
            int final_stack = Mathf.Clamp(randomized_stack, 1, item_data.max_stack_size);

            Inventory_Item item_to_add = new Inventory_Item(item_data);
            item_to_add.stack_size = final_stack;

            possible_items.Add(item_to_add);
        }

        int random_item_amount = Random.Range(min_items_amount, max_inventory_size + 1);
        int final_amount = Mathf.Clamp(random_item_amount, 1, possible_items.Count);

        for (int i = 0; i < final_amount;i++)
        {
            var random_index = Random.Range(0, possible_items.Count);
            var item = possible_items[random_index];

            if (CanAddItem(item))
            {
                possible_items.Remove(item);
                AddItem(item);
            }
        }

        TriggerUpdateUI();
    }


    public void SetInventory(Inventory_Player inventory) => this.inventory = inventory;
}
