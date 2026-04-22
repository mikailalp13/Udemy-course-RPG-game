using UnityEngine;

public class Object_Checkpoint : MonoBehaviour, ISaveable
{
    [SerializeField] private string checkpoint_id;
    [SerializeField] private Transform respawn_point;
    private Animator anim;
    private AudioSource fire_audio_source;
    public bool is_active { get; private set;}



    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        fire_audio_source = GetComponent<AudioSource>();
    }


    public string GetCheckPointId() => checkpoint_id;

    public Vector3 GetPosition() => respawn_point == null ? transform.position : respawn_point.position;


    public void ActivateCheckPoint(bool activate)
    {
        is_active = activate;
        anim.SetBool("isActive", activate);

        if (is_active && fire_audio_source.isPlaying == false)
            fire_audio_source.Play();
            
        if (is_active == false)
            fire_audio_source.Stop();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        ActivateCheckPoint(true);
    }


    public void LoadData(GameData data)
    {
        bool active = data.unlocked_checkpoints.TryGetValue(checkpoint_id, out active);
        ActivateCheckPoint(active);
    }


    public void SaveData(ref GameData data)
    {
        if (is_active == false)
            return;

        if (data.unlocked_checkpoints.ContainsKey(checkpoint_id) == false)
            data.unlocked_checkpoints.Add(checkpoint_id, true);
    }


    private void OnValidate()
    {
#if UNITY_EDITOR
        if (string.IsNullOrEmpty(checkpoint_id))
        {
            checkpoint_id = System.Guid.NewGuid().ToString();
        }
#endif
    }
}
