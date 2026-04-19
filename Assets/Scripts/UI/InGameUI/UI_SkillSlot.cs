using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private UI ui;
    private Image skill_icon;
    private RectTransform rect;
    private Button button;

    private Skill_DataSO skill_data;

    public SkillType skill_type;
    [SerializeField] private Image cooldown_image;
    [SerializeField] private string input_key_name;
    [SerializeField] private TextMeshProUGUI input_key_text;
    [SerializeField] private GameObject conflict_slot;


    private void Awake()
    {
        ui = GetComponentInParent<UI>();
        button = GetComponent<Button>();
        skill_icon = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
    }

    private void OnValidate()
    {
        gameObject.name = "UI_SkillSlot - " + skill_type.ToString();
    }

    public void SetupSkillSlot(Skill_DataSO selected_skill)
    {
        this.skill_data = selected_skill;

        Color color = Color.black; color.a = 0.6f;
        cooldown_image.color = color;

        input_key_text.text = input_key_name;
        skill_icon.sprite = selected_skill.icon;

        if (conflict_slot != null)
            conflict_slot.SetActive(false);
    }

    public void StartCooldown(float cooldown)
    {
        cooldown_image.fillAmount = 1;
        StartCoroutine(CooldownCo(cooldown));
    }

    public void ResetCooldown() => cooldown_image.fillAmount = 0;

    private IEnumerator CooldownCo(float duration)
    {
        float time_passed = 0;

        while (time_passed < duration)
        {
            time_passed += Time.deltaTime;
            cooldown_image.fillAmount = 1f - (time_passed / duration);
            yield return null;
        }

        cooldown_image.fillAmount = 0;
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skill_tool_tip.ShowToolTip(false, null);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (skill_data == null)
            return;
            
        ui.skill_tool_tip.ShowToolTip(true, rect, skill_data, null);
    }
}
