using UnityEngine;

public class UI_ToolTip : MonoBehaviour
{
    private RectTransform rect;

    [SerializeField] private Vector2 offset = new Vector2(300, 20);

    protected virtual void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public virtual void ShowToolTip(bool show, RectTransform targetRect)
    {
        if (show == false)
        {
            rect.position = new Vector2(99999, 99999);
            return;
        }

        UpdatePosition(targetRect);
    }

    private void UpdatePosition(RectTransform targetRect)
    {
        float screen_center_x = Screen.width / 2f;
        float screen_top = Screen.height;
        float screen_bottom = 0;

        Vector2 target_position = targetRect.position;

        target_position.x = target_position.x > screen_center_x ? target_position.x - offset.x : target_position.x + offset.x;

        float vertical_half = rect.sizeDelta.y / 2;
        float top_y = target_position.y + vertical_half;
        float bottom_y = target_position.y - vertical_half;

        if (top_y > screen_top)
            target_position.y = screen_top - vertical_half - offset.y;
        else if (bottom_y < screen_bottom)
            target_position.y = screen_bottom + vertical_half + offset.y;

        rect.position = target_position;
    }

    protected string GetColoredText(string color, string text)
    {
        return $"<color={color}>{text}</color>";
    }
}
