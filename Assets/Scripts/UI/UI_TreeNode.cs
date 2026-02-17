using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private UI ui;
    private RectTransform rect;
    private UI_SkillTree skill_tree;
    private UI_TreeConnectHandler connect_handler;


    [Header("Unlock Details")]
    public UI_TreeNode[] needed_nodes;
    public UI_TreeNode[] conflict_nodes;
    public bool is_unlocked;
    public bool is_locked;

    [Header("Skill Details")]
    public Skill_DataSO skill_data;
    [SerializeField] private string skill_name;
    [SerializeField] private Image skill_icon;
    [SerializeField] private int skill_cost;
    [SerializeField] private string locked_color_hex = "#9F9797";
    private Color last_color;


    private void Awake()
    {
        ui = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>();
        skill_tree = GetComponentInParent<UI_SkillTree>();
        connect_handler = GetComponent<UI_TreeConnectHandler>();

        UpdateIconColor(GetColorByHex(locked_color_hex));
    }

    public void Refund()
    {
        is_unlocked = false;
        is_locked = false;
        UpdateIconColor(GetColorByHex(locked_color_hex));

        skill_tree.AddSkillPoints(skill_data.cost);
        connect_handler.UnlockConnectionImage(false);
    }

    private void Unlock()
    {
        is_unlocked = true;
        UpdateIconColor(Color.white);
        LockConflictNodes();

        skill_tree.RemoveSkillPoints(skill_data.cost);
        connect_handler.UnlockConnectionImage(true);
    }

    private bool CanBeUnlocked()
    {
        if (is_locked || is_unlocked)
            return false;

        if (skill_tree.EnoughSkillPoints(skill_data.cost) == false)
            return false;

        foreach (var node in needed_nodes)
        {
            if (node.is_unlocked == false)
                return false;
        }

        foreach (var node in conflict_nodes)
        {
            if (node.is_unlocked)
                return false;
        }

        return true;
    }

    private void LockConflictNodes()
    {
        foreach (var node in conflict_nodes)  
            node.is_locked = true; 
    }

    private void UpdateIconColor(Color color)
    {
        if (skill_icon == null)
            return;
        
        last_color = skill_icon.color;
        skill_icon.color = color;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (CanBeUnlocked())
            Unlock();
        else if (is_locked)
            ui.skill_tool_tip.LockedSkillEffect();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skill_tool_tip.ShowToolTip(true, rect, this);

        if (is_unlocked == false || is_locked == false)
            ToggleNodeHighlight(true);        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skill_tool_tip.ShowToolTip(false, rect);

        if (is_unlocked == false || is_locked == false)
            ToggleNodeHighlight(false);
    }

    private void ToggleNodeHighlight(bool highlight)
    {
        Color highlight_color = Color.white * 0.9f; highlight_color.a = 1; // no transparency
        Color color_to_apply = highlight ? highlight_color : last_color;

        UpdateIconColor(color_to_apply);
    }

    private Color GetColorByHex(string hexNumber)
    {
        ColorUtility.TryParseHtmlString(hexNumber, out Color color);

        return color;
    }


    private void OnDisable()
    {
        if (is_locked)
            UpdateIconColor(GetColorByHex(locked_color_hex));
        if (is_unlocked)
            UpdateIconColor(Color.white);
    }

    private void OnValidate()
    {
        if (skill_data == null)
            return;
        skill_name = skill_data.display_name;
        skill_icon.sprite = skill_data.icon;
        skill_cost = skill_data.cost;
        gameObject.name = "UI_TreeNode - " + skill_data.display_name;
    }

}
