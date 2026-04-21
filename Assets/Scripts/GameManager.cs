using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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


    public void ChangeScene(string scene_name, RespawnType respawn_type)
    {
        StartCoroutine(ChangeSceneCO(scene_name,respawn_type));
    }


    private IEnumerator ChangeSceneCO(string scene_name, RespawnType respawn_type)
    {
        // fade effect
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(scene_name);

        yield return new WaitForSeconds(0.2f);

        Vector3 position = GetWaypointPosition(respawn_type);

        if (position != Vector3.zero)
            Player.instance.TeleportPlayer(position);
    }


    private Vector3 GetWaypointPosition(RespawnType type)
    {
        var waypoints = FindObjectsByType<Object_Waypoint>(FindObjectsSortMode.None);

        foreach (var point in waypoints)
        {
            if (point.GetWaypointType() == type)
                return point.GetPosition();
        }

        return Vector3.zero;
    }
}
