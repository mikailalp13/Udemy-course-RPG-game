using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

[Serializable]
public class UI_TreeConnectDetails
{
    public UI_TreeConnectHandler child_node;
    public NodeDirectionType direction;
    [Range(100f, 350f)] public float length;
    [Range(-50f, 50f)] public float rotation;
}

public class UI_TreeConnectHandler : MonoBehaviour
{
    private RectTransform rect => GetComponent<RectTransform>();
    [SerializeField] private UI_TreeConnectDetails[] connection_details;
    [SerializeField] private UI_TreeConnection[] connections;  
    private Image connection_image;
    private Color original_color;

    private void Awake()
    {
        if (connection_image != null)
            original_color = connection_image.color;
    }

    public UI_TreeNode[] GetChildNodes()
    {
        List<UI_TreeNode> children_to_return = new List<UI_TreeNode>();

        foreach (var node in connection_details)
        {
            if (node.child_node != null)
            {
                children_to_return.Add(node.child_node.GetComponent<UI_TreeNode>());
            }
        }

        return children_to_return.ToArray();
    }

    public void UpdateConnections()
    {
        for(int i = 0; i < connection_details.Length; i++)
        {
            var detail = connection_details[i];
            var connection = connections[i];

            Vector2 target_posiiton = connection.GetConnectionPoint(rect);
            Image connection_image = connection.GetConnectionImage();
            
            connection.DirectConnection(detail.direction, detail.length, detail.rotation);

            if (detail.child_node == null)
                continue;

            detail.child_node.SetPosition(target_posiiton);
            detail.child_node.SetConnectionImage(connection_image);
            detail.child_node.transform.SetAsLastSibling();
        }
    }

    public void UpdateAllConnections()
    {
        UpdateConnections();

        foreach (var node in connection_details)
        {
            if (node.child_node == null)   
                continue;

            node.child_node.UpdateConnections();
        }
    }

    public void UnlockConnectionImage(bool unlocked)
    {
        if (connection_image == null)
            return;
        
        connection_image.color = unlocked ? Color.white : original_color;
    }
    public void SetConnectionImage(Image image) => connection_image = image;   
    public void SetPosition(Vector2 position) => rect.anchoredPosition = position;
    private void OnValidate()
    {
        if (connection_details.Length <= 0)
            return;
        if (connection_details.Length != connections.Length)
        {
            return;
        }
        UpdateConnections();
    }
}
