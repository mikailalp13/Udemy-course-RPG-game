using UnityEngine;

public class Object_ItemPickup : MonoBehaviour
{
    [SerializeField] private Vector2 drop_force = new Vector2(3, 10);
    [SerializeField] private ItemDataSO item_data;

    [Space]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D col;



    private void OnValidate()
    {
        if (item_data == null)
            return;

        sr = GetComponent<SpriteRenderer>();
        SetupVisuals();
    }

    public void SetupItem(ItemDataSO item_data)
    {
        this.item_data = item_data;

        SetupVisuals();

        float x_drop_force = Random.Range(-drop_force.x, drop_force.x);
        rb.linearVelocity = new Vector2(x_drop_force, drop_force.y);
        col.isTrigger = false;
    }

    private void SetupVisuals()
    {
        sr.sprite = item_data.item_icon;
        gameObject.name = "Object_ItemPickup - " + item_data.item_name;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && col.isTrigger == false)
        {
            col.isTrigger = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Inventory_Player inventory = collision.GetComponent<Inventory_Player>();

        if (inventory == null)
            return;

        Inventory_Item item_to_add = new Inventory_Item(item_data);
        Inventory_Storage storage = inventory.storage;

        if (item_data.item_type == ItemType.Material)
        {
            storage.AddMaterialToStash(item_to_add);
            Destroy(gameObject);
            return;
        }

        if (inventory.CanAddItem(item_to_add))
        {
            inventory.AddItem(item_to_add);
            Destroy(gameObject);
        }
    }
}
