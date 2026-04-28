using UnityEngine;

public class Object_NPC : MonoBehaviour, IInteractable
{
    protected UI ui; 
    protected Transform player;
    protected Player_QuestManager quest_manager;

    [Header("Quest Info")]
    [SerializeField] private string npc_target_quest_id;
    [SerializeField] private RewardType reward_npc;

    [Space]
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


    protected virtual void Start()
    {
        quest_manager = Player.instance.quest_manager;
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

    public virtual void Interact()
    {
        quest_manager.AddProgress(npc_target_quest_id);
        quest_manager.TryGiveRewardFrom(reward_npc);
    }
}
