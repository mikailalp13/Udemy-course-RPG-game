using UnityEngine;
using UnityEngine.SceneManagement;

public class Object_Portal : MonoBehaviour, ISaveable
{
    public static Object_Portal instance;
    public bool is_active { get; private set; }

    [SerializeField] private Vector2 default_position; // Where portal appears in town
    [SerializeField] private string town_scene_name = "Level_0";

    [SerializeField] private Transform respawn_point;
    [SerializeField] private bool can_be_triggered;

    private string current_scene_name;
    private string return_scene_name;
    private bool returning_from_town;


    private void Awake()
    {
        instance = this;
        current_scene_name = SceneManager.GetActiveScene().name;
        transform.position = new Vector3(9999, 9999); // Hide by default
    }


    public void ActivatePortal(Vector3 position, int facing_dir = 1)
    {
        is_active = true;
        transform.position = position;
        SaveManager.instance.GetGameData().in_scene_portals.Clear();

        if (facing_dir == -1)
            transform.Rotate(0, 180, 0);
    }


    public void DisableIfNeeded()
    {
        if (returning_from_town == false)
            return;
        
        SaveManager.instance.GetGameData().in_scene_portals.Remove(current_scene_name);
        is_active = false;
        transform.position = new Vector3(9999, 9999);
    }


    private void UseTeleport()
    {
        string destination_scene = InTown() ? return_scene_name : town_scene_name;

        GameManager.instance.ChangeScene(destination_scene, RespawnType.Portal);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (can_be_triggered == false)
            return;

        UseTeleport();
    }


    private void OnTriggerExit2D(Collider2D collision) => can_be_triggered = true;
    
    public void SetTrigger(bool trigger) => can_be_triggered = trigger;

    public Vector3 GetPosition() => respawn_point != null ? respawn_point.position : transform.position;

    private bool InTown() => current_scene_name == town_scene_name;


    public void LoadData(GameData data)
    {
        if (InTown() && data.in_scene_portals.Count > 0)
        {
            transform.position = default_position;
            is_active = true;
        }
        else if (data.in_scene_portals.TryGetValue(current_scene_name, out Vector3 portal_position))
        {
            transform.position = portal_position;
            is_active = true;
        }

        returning_from_town = data.returning_from_town;
        return_scene_name = data.portal_destination_scene_name;
    }


    public void SaveData(ref GameData data)
    {
        data.returning_from_town = InTown();

        if (is_active && InTown() == false)
        {
            data.in_scene_portals[current_scene_name] = transform.position;
            data.portal_destination_scene_name = current_scene_name;
        }
        else
        {
            data.in_scene_portals.Remove(current_scene_name);
        }
    }
}
