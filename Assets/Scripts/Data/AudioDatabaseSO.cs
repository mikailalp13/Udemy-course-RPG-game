using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Audio / Audio Database")]

public class AudioDatabaseSO : ScriptableObject
{
    public List<AudioClipData> player_audio;
    public List<AudioClipData> ui_audio;

    [Header("Music Lists")]
    public List<AudioClipData> main_menu_music;
    public List<AudioClipData> level_music;


    private Dictionary<string, AudioClipData> clip_collection;


    private void OnEnable()
    {
        clip_collection = new Dictionary<string, AudioClipData>();

        AddToCollection(player_audio);
        AddToCollection(ui_audio);
        AddToCollection(main_menu_music);
        AddToCollection(level_music);
    }


    public AudioClipData Get(string group_name)
    {
        return clip_collection.TryGetValue(group_name, out var data) ? data : null;
    }


    private void AddToCollection(List<AudioClipData> list_to_add)
    {
        foreach (var data in list_to_add)
        {
            if (data != null && clip_collection.ContainsKey(data.audio_name) == false)
            {
                clip_collection.Add(data.audio_name, data);
            }
        }
    }
}


[System.Serializable]
public class AudioClipData
{
    public string audio_name;
    public List<AudioClip> clips = new List<AudioClip>();
    [Range(0f, 1f)] public float max_volume = 1f;


    public AudioClip GetRandomClip()
    {
        if (clips == null || clips.Count == 0)
        {
            Debug.Log("No audio found!");
            return null;
        }
        else
            return clips[Random.Range(0, clips.Count)];
    }
}
