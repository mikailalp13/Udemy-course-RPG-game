using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject[] ui_elements;
    public bool alternative_input { get; private set; }
    private PlayerInputSet input;

    #region UI Components
    public UI_SkillToolTip skill_tool_tip { get; private set; }
    public UI_ItemToolTip item_tool_tip { get; private set; }
    public UI_StatToolTip stat_tool_tip { get; private set; }

    public UI_SkillTree skill_tree_ui { get; private set; }
    public UI_Inventory inventory_ui { get; private set; }
    public UI_Storage storage_ui { get; private set; }
    public UI_Craft craft_ui { get; private set; }
    public UI_Merchant merchant_ui { get; private set; }
    public UI_InGame in_game_ui { get; private set; }
    public UI_Options options_ui { get; private set; }

    #endregion

    private bool skill_tree_enabled;
    private bool inventory_enabled;

    private void Awake()
    {
        item_tool_tip = GetComponentInChildren<UI_ItemToolTip>();
        stat_tool_tip = GetComponentInChildren<UI_StatToolTip>();
        skill_tool_tip = GetComponentInChildren<UI_SkillToolTip>();


        skill_tree_ui = GetComponentInChildren<UI_SkillTree>(true); // when you give true to the function it works even when the game object is disabled
        inventory_ui = GetComponentInChildren<UI_Inventory>(true);
        storage_ui = GetComponentInChildren<UI_Storage>(true);
        craft_ui = GetComponentInChildren<UI_Craft>(true);
        merchant_ui = GetComponentInChildren<UI_Merchant>(true);
        in_game_ui = GetComponentInChildren<UI_InGame>(true);
        options_ui = GetComponentInChildren<UI_Options>(true);

        skill_tree_enabled = skill_tree_ui.gameObject.activeSelf;
        inventory_enabled = inventory_ui.gameObject.activeSelf;
    }

    private void Start()
    {
        skill_tree_ui.UnlockDefaultSkills();
    }


    public void SetupControlsUI(PlayerInputSet input_set)
    {
        input = input_set;

        input.UI.InventoryUI.performed += ctx => ToggleInventoryUI();
        input.UI.SkillTreeUI.performed += ctx => ToggleSkillTreeUI();

        input.UI.AlternativeInput.performed += ctx => alternative_input = true;
        input.UI.AlternativeInput.canceled += ctx => alternative_input = false;

        input.UI.OptionsUI.performed += ctx =>
        {
            foreach (var element in ui_elements)
            {
                if (element.activeSelf)
                {
                    Time.timeScale = 1;
                    SwitchToInGameUI();
                    return;
                }
            }
            Time.timeScale = 0;
            OpenOptionsUI();
        };
    }


    public void OpenOptionsUI()
    {
        foreach (var element in ui_elements)
            element.gameObject.SetActive(false);

        HideAllTooltips();
        StopPlayerControls(true);
        options_ui.gameObject.SetActive(true);
    }


    public void SwitchToInGameUI()
    {
        foreach (var element in ui_elements)
            element.gameObject.SetActive(false);


        HideAllTooltips();
        StopPlayerControls(false);
        in_game_ui.gameObject.SetActive(true);

        inventory_enabled = false;
        skill_tree_enabled = false;
    }


    private void StopPlayerControls(bool stop_controls)
    {
        if (stop_controls)
            input.Player.Disable();
        else
            input.Player.Enable();
    }


    private void StopPlayerControlsIfNeeded()
    {
        foreach (var element in ui_elements)
        {
            if (element.activeSelf)
            {
                StopPlayerControls(true);
                return;
            }
        }

        StopPlayerControls(false);
    }


    public void ToggleSkillTreeUI()
    {
        skill_tree_ui.transform.SetAsLastSibling();
        SetTooltipsAsLastSibling();

        skill_tree_enabled = !skill_tree_enabled;
        skill_tree_ui.gameObject.SetActive(skill_tree_enabled);

        HideAllTooltips();
        StopPlayerControlsIfNeeded();
    }

    public void ToggleInventoryUI()
    {
        inventory_ui.transform.SetAsLastSibling();
        SetTooltipsAsLastSibling();

        inventory_enabled = !inventory_enabled;
        inventory_ui.gameObject.SetActive(inventory_enabled);

        HideAllTooltips();
        StopPlayerControlsIfNeeded();
    }


    public void OpenStorageUI(bool open_storage_ui)
    {
        storage_ui.gameObject.SetActive(open_storage_ui);
        StopPlayerControls(open_storage_ui);

        if (open_storage_ui == false)
        {
            craft_ui.gameObject.SetActive(false);
            HideAllTooltips();
        }
    }


    public void OpenMerchantUI(bool open_merchant_ui)
    {
        merchant_ui.gameObject.SetActive(open_merchant_ui);
        StopPlayerControls(open_merchant_ui);

        if (open_merchant_ui == false)
            HideAllTooltips();
    }


    public void HideAllTooltips()
    {
        item_tool_tip.ShowToolTip(false, null);
        skill_tool_tip.ShowToolTip(false, null);
        stat_tool_tip.ShowToolTip(false, null);
    }


    private void SetTooltipsAsLastSibling()
    {
        item_tool_tip.transform.SetAsLastSibling();
        skill_tool_tip.transform.SetAsLastSibling();
        stat_tool_tip.transform.SetAsLastSibling();
    }
}
