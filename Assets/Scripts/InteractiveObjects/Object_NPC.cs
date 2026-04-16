using UnityEngine;

public class Object_NPC : MonoBehaviour
{
    protected Transform player;
    protected UI ui; 


    [SerializeField] private Transform npc;
    [SerializeField] private GameObject interact_tool_tip;
    private bool facing_right = true;


    [Header("Floaty Tooltip")]
    [SerializeField] private float float_speed = 8f;
    [SerializeField] private float float_range = 0.1f;
    private Vector3 start_position;


    protected virtual void Awake()
    {
        ui = FindFirstObjectByType<UI>();
        start_position = interact_tool_tip.transform.position;
        interact_tool_tip.SetActive(false);
    }


    protected virtual void Update()
    {
        HandleNpcFlip();
        HandleTooltipFloat();
    }

    private void HandleTooltipFloat()
    {
        if (interact_tool_tip.activeSelf)
        {
            float y_offset = Mathf.Sin(Time.time * float_speed) * float_range;
            interact_tool_tip.transform.position = start_position + new Vector3(0, y_offset);
        }
    }


    private void HandleNpcFlip()
    {
        if (player == null || npc == null)
            return;
        
        if (npc.position.x > player.position.x && facing_right)
        {
            npc.transform.Rotate(0, 180, 0);
            facing_right = false;    
        }
        else if (npc.position.x < player.position.x && facing_right == false)
        {
            npc.transform.Rotate(0, 180, 0);
            facing_right = true;
        }
    }


    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        player = collision.transform;
        interact_tool_tip.SetActive(true);
    }


    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        interact_tool_tip.SetActive(false);
    }
}
