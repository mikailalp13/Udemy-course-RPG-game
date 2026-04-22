using UnityEngine;
using System.Linq;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveable
{
    public static GameManager instance;
    private Vector3 last_player_position;

    private string last_scene_played;
    private bool data_loaded;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }


    // public void SetLastPlayerPosition(Vector3 position) => last_player_position = position;


    public void ContinuePlay()
    {
        //ChangeScene(last_scene_played, RespawnType.NoneSpecific);
        if (string.IsNullOrEmpty(last_scene_played))
        {
            Debug.Log("No save found, loading default scene.");
            last_scene_played = "Level_0";
        }

        ChangeScene(last_scene_played, RespawnType.NoneSpecific);
    }


    public void RestartScene()
    {
        string scene_name = SceneManager.GetActiveScene().name;
        ChangeScene(scene_name, RespawnType.NoneSpecific);
    }


    public void ChangeScene(string scene_name, RespawnType respawn_type)
    {
        SaveManager.instance.SaveGame();
        
        Time.timeScale = 1;
        StartCoroutine(ChangeSceneCO(scene_name,respawn_type));
    }


    private IEnumerator ChangeSceneCO(string scene_name, RespawnType respawn_type)
    {
        UI_FadeScreen fade_screen = FindFadeScreenUI();
        
        fade_screen.DoFadeOut();
        yield return fade_screen.fade_effect_co;

        SceneManager.LoadScene(scene_name);

        data_loaded = false; //data_loaded becomes true when you load the game from save manager
        yield return null; // one frame delay

        while (data_loaded == false)
        {
            yield return null;
        }

        fade_screen = FindFadeScreenUI(); // since we're loading another scene we'll lost the reference
        fade_screen.DoFadeIn();

        Player player = Player.instance;

        if (player == null)
            yield break;

        Vector3 position = GetNewPlayerPosition(respawn_type);

        if (position != Vector3.zero)
            player.TeleportPlayer(position);
    }


    private UI_FadeScreen FindFadeScreenUI()
    {
        if (UI.instance != null)
            return UI.instance.fade_screen_ui;
        else
            return FindFirstObjectByType<UI_FadeScreen>();
    }


    private Vector3 GetNewPlayerPosition(RespawnType type)
    {
        if (type == RespawnType.Portal)
        {
            Object_Portal portal = Object_Portal.instance;

            Vector3 position = portal.GetPosition();

            portal.SetTrigger(false);
            portal.DisableIfNeeded();

            return position;
        }

        if (type == RespawnType.NoneSpecific)
        {
            var data = SaveManager.instance.GetGameData();
            var checkpoints = FindObjectsByType<Object_Checkpoint>(FindObjectsSortMode.None);

            var unlocked_checkpoints = checkpoints
                .Where(cp => data.unlocked_checkpoints.TryGetValue(cp.GetCheckPointId(), out bool unlocked) && unlocked)
                .Select(cp => cp.GetPosition())
                .ToList();
            
            var enter_waypoints = FindObjectsByType<Object_Waypoint>(FindObjectsSortMode.None)
                .Where(wp => wp.GetWaypointType() == RespawnType.Enter)
                .Select(wp => wp.GetPositionAndSetTriggerFalse())
                .ToList();

            var selected_positions = unlocked_checkpoints.Concat(enter_waypoints).ToList(); // combine two lists into one

            if (selected_positions.Count == 0)
                return Vector3.zero;
            
            // order the list by distance so when player dies and come back to game it will start from the nearest checkpoint or enter gate
            return selected_positions
                .OrderBy(position => Vector3.Distance(position, last_player_position)) // arrange from lowest to highest by comparing distance
                .First(); 
        }

        return GetWaypointPosition(type);
    }


    private Vector3 GetWaypointPosition(RespawnType type)
    {
        var waypoints = FindObjectsByType<Object_Waypoint>(FindObjectsSortMode.None);

        foreach (var point in waypoints)
        {
            if (point.GetWaypointType() == type)
                return point.GetPositionAndSetTriggerFalse();
        }

        return Vector3.zero;
    }

    public void LoadData(GameData data)
    {
        last_scene_played = data.last_scene_played;
        last_player_position = data.last_player_position;

        if (string.IsNullOrEmpty(last_scene_played))
            last_scene_played = "Level_0";
        
        data_loaded = true;
    }

    public void SaveData(ref GameData data)
    {
        string current_scene = SceneManager.GetActiveScene().name;

        if (current_scene == "MainMenu")
            return;

        data.last_player_position = Player.instance.transform.position;
        data.last_scene_played = current_scene;
        data_loaded = false;
    }
}
