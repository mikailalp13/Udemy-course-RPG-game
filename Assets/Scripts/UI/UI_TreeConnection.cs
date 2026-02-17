using UnityEngine;
using UnityEngine.UI;
using System;

public class UI_TreeConnection : MonoBehaviour
{
    [SerializeField] private RectTransform rotation_point;
    [SerializeField] private RectTransform connection_length;
    [SerializeField] private RectTransform child_node_connection_point;

    public void DirectConnection(NodeDirectionType direction, float length, float offset)
    {
        bool should_be_active = direction != NodeDirectionType.None;
        float final_length = should_be_active ? length : 0;
        float angle = GetDirectionAngle(direction);

        rotation_point.localRotation = Quaternion.Euler(0, 0, angle + offset);
        connection_length.sizeDelta = new Vector2(final_length, connection_length.sizeDelta.y);
    }

    public Image GetConnectionImage() => connection_length.GetComponent<Image>();

    
    public Vector2 GetConnectionPoint(RectTransform rect)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle
        (
            rect.parent as RectTransform,
            child_node_connection_point.position,
            null, //camera
            out var localPosition
        );

        return localPosition;
    }

    private float GetDirectionAngle(NodeDirectionType type)
    {
        switch (type)
        {
            case NodeDirectionType.UpLeft: return 135f;
            case NodeDirectionType.Up: return 90f;
            case NodeDirectionType.UpRight: return 45f;
            case NodeDirectionType.Left: return 180f;
            case NodeDirectionType.Right: return 0f;
            case NodeDirectionType.DownLeft: return -135f;
            case NodeDirectionType.Down: return -90f;
            case NodeDirectionType.DownRight: return -45f;
            default: return 0f;
        }
    }
}
public enum NodeDirectionType
{
    None,
    UpLeft,
    Up,
    UpRight,
    Left,
    Right,
    DownLeft,
    Down,
    DownRight
}