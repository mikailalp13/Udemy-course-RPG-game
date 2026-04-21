using UnityEngine;

public class Object_Checkpoint : MonoBehaviour, ISaveable
{
    private Animator anim;
    private Object_Checkpoint[] all_checkpoints;


    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        
        all_checkpoints = FindObjectsByType<Object_Checkpoint>(FindObjectsSortMode.None);
    }


    public void ActivateCheckPoint(bool activate)
    {
        anim.SetBool("isActive", activate);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (var point in all_checkpoints)
            point.ActivateCheckPoint(false);
        
        SaveManager.instance.GetGameData().saved_check_point = transform.position;
        ActivateCheckPoint(true);
    }

    public void LoadData(GameData data)
    {
        bool active = data.saved_check_point == transform.position;
        ActivateCheckPoint(active);

        if (active)
            Player.instance.TeleportPlayer(transform.position);
    }

    public void SaveData(ref GameData data)
    {
        
    }
}
