using UnityEngine;

public class Object_ItemPickup : MonoBehaviour
{
    private SpriteRenderer sr;
    [SerializeField] private ItemDataSO item_data;

    private Inventory_Item item_to_add;
    private Inventory_Base inventory;


    private void Awake()
    {
        item_to_add = new Inventory_Item(item_data);
    }


    private void OnValidate()
    {
        if (item_data == null)
            return;

        sr = GetComponent<SpriteRenderer>();
        sr.sprite = item_data.item_icon;
        gameObject.name = "Object_ItemPickup - " + item_data.item_name;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        inventory = collision.GetComponent<Inventory_Base>();

        if (inventory == null)
            return;

        bool can_add_item = inventory.CanAddItem() || inventory.FindStackable(item_to_add) != null;

        if (can_add_item)
        {
            inventory.AddItem(item_to_add);
            Destroy(gameObject);
        }
    }
}
