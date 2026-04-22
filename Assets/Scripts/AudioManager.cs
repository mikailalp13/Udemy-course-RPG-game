using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioDatabaseSO audio_database;
    [SerializeField] private AudioSource bgm_source;
    [SerializeField] private AudioSource sfx_source;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // so that when the scene changes music wont stop
    }


    public void PlaySFX(string sound_name, AudioSource sfx_source)
    {
        AudioClipData data = audio_database.Get(sound_name);

        if (data == null)
        {
            Debug.Log("Attempt to play sound - " + sound_name);
            return;
        }
        
        AudioClip clip = data.GetRandomClip();

        if (clip == null)
            return;

        sfx_source.clip = clip;
        sfx_source.PlayOneShot(clip);
    }
}
