using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class Entity_DropManager : MonoBehaviour
{
    [SerializeField] private GameObject item_drop_prefab;
    [SerializeField] private ItemListDataSO drop_data;

    [Header("Drop Restrictions")]
    [SerializeField] private int max_rarity_amount = 1200;
    [SerializeField] private int max_items_to_drop = 3;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
            DropItems();
    }

    public virtual void DropItems()
    {
        if (drop_data == null)
        {
            Debug.Log("You need to assign drop data on entity: " + gameObject.name);
            return;
        }

        List<ItemDataSO> items_to_drop = RollDrops();
        int amount_to_drop = Mathf.Min(items_to_drop.Count, max_items_to_drop);

        for (int i = 0; i < amount_to_drop; i++)
        {
            CreateItemDrop(items_to_drop[i]);
        }
    }


    protected void CreateItemDrop(ItemDataSO item_to_drop)
    {
        GameObject new_item = Instantiate(item_drop_prefab, transform.position, Quaternion.identity);
        new_item.GetComponent<Object_ItemPickup>().SetupItem(item_to_drop);
    }

    public List<ItemDataSO> RollDrops()
    {
        List<ItemDataSO> possible_drops = new List<ItemDataSO>();
        List<ItemDataSO> final_drops = new List<ItemDataSO>();
        float max_rarity_amount = this.max_rarity_amount;

        // Step 1: Roll each item based on rarity and max drop chance.
        foreach (var item in drop_data.item_list)
        {
            float drop_chance = item.GetDropChance();

            if (Random.Range(0, 100) <= drop_chance)
                possible_drops.Add(item);
        }

        // Step 2: Sort by rarity (highest to lowest).
        possible_drops = possible_drops.OrderByDescending(item => item.item_rarity).ToList();

        // Step 3: Add items to final drop list until rarity limit on entity is reached.

        foreach (var item in possible_drops)
        {
            if (max_rarity_amount > item.item_rarity)
            {
                final_drops.Add(item);
                max_rarity_amount -= item.item_rarity;
            }
        }

        return final_drops;
    } 
}
