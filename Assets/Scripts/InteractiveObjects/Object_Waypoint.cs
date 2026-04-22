using UnityEngine;
using UnityEngine.SceneManagement;

public class Object_Waypoint : MonoBehaviour
{
    [SerializeField] private string transfer_to_scene;

    [Space]
    [SerializeField] private RespawnType waypoint_type;
    [SerializeField] private RespawnType connected_waypoint; // if you enter the enter gate, you'll go the the previous scene's exit gate
    [SerializeField] private Transform respawn_point;
    [SerializeField] private bool can_be_triggered = true;


    public RespawnType GetWaypointType() => waypoint_type;

    public Vector3 GetPositionAndSetTriggerFalse()
    {
        can_be_triggered = false;
        return respawn_point == null ? transform.position : respawn_point.position;
    }


    private void OnValidate()
    {
        gameObject.name = "Object_Waypoint - " + waypoint_type.ToString() + " - " + transfer_to_scene;

        if (waypoint_type == RespawnType.Enter)
            connected_waypoint = RespawnType.Exit;

        else if (waypoint_type == RespawnType.Exit)
            connected_waypoint = RespawnType.Enter;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (can_be_triggered == false)
            return;

        GameManager.instance.ChangeScene(transfer_to_scene, connected_waypoint);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        can_be_triggered = true;
    }
}
