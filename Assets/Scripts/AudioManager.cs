using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioDatabaseSO audio_database;
    [SerializeField] private AudioSource bgm_source;
    [SerializeField] private AudioSource sfx_source;

    [Space]
    private Transform player;
    private AudioClip last_music_played;
    private string current_bgm_group_name;
    private Coroutine current_bgm_co;
    [SerializeField] private bool bgm_should_play;


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


    private void Update()
    {
        if (bgm_source.isPlaying == false && bgm_should_play)
        {
            if (string.IsNullOrEmpty(current_bgm_group_name) == false)
                NextBGM(current_bgm_group_name);
        }

        if (bgm_source.isPlaying && bgm_should_play == false)
            StopBGM();
    }


    public void StartBGM(string music_group)
    {
        bgm_should_play = true;
        
        if (music_group == current_bgm_group_name)
            return;
        
        NextBGM(music_group);
    }


    public void NextBGM(string music_group)
    {
        bgm_should_play = true;
        current_bgm_group_name = music_group;

        if (current_bgm_co != null)
            StopCoroutine(current_bgm_co);

        current_bgm_co = StartCoroutine(SwitchMusicCo(music_group));
    }


    public void StopBGM()
    {
        bgm_should_play = false;

        StartCoroutine(FadeVolumeCo(bgm_source, 0, 1));

        if (current_bgm_co != null)
            StopCoroutine(current_bgm_co);
    }


    private IEnumerator SwitchMusicCo(string music_group)
    {
        AudioClipData data = audio_database.Get(music_group);

        AudioClip next_music = data.GetRandomClip();

        if (data == null || data.clips.Count == 0)
        {
            Debug.Log("No audio found for group - " + music_group);
            yield break;
        }    

        if (data.clips.Count > 1)
        {
            while (next_music == last_music_played)
                next_music = data.GetRandomClip();
        }

        if (bgm_source.isPlaying)
            yield return FadeVolumeCo(bgm_source, 0, 1f);

        last_music_played = next_music;
        bgm_source.clip = next_music;
        bgm_source.volume = 0;
        bgm_source.Play();

        StartCoroutine(FadeVolumeCo(bgm_source, data.max_volume, 1f));
    }


    private IEnumerator FadeVolumeCo(AudioSource source, float target_volume, float duration)
    {
        float time = 0f;
        float start_volume = source.volume;

        while (time < duration)
        {
            time += Time.deltaTime;

            source.volume = Mathf.Lerp(start_volume, target_volume, time / duration);
            yield return null;
        }

        source.volume = target_volume;
    }


    public void PlaySFX(string sound_name, AudioSource sfx_source, float min_distance_to_hear_sounds = 5f)
    {
        if (player == null)
            player = Player.instance.transform;

        AudioClipData data = audio_database.Get(sound_name);

        if (data == null)
        {
            Debug.Log("Attempt to play sound - " + sound_name);
            return;
        }
        
        AudioClip clip = data.GetRandomClip();

        if (clip == null)
            return;

        float max_volume = data.max_volume;
        float distance = Vector2.Distance(sfx_source.transform.position, player.position);
        float t = Mathf.Clamp01(1 - (distance / min_distance_to_hear_sounds));

        sfx_source.pitch = Random.Range(0.8f, 1.3f);
        sfx_source.volume = Mathf.Lerp(0, max_volume, t * t);
        sfx_source.PlayOneShot(clip);
    }


    public void PlayGlobalSFX(string sound_name)
    {
        AudioClipData data = audio_database.Get(sound_name);

        if (data == null)
            return;

        AudioClip clip = data.GetRandomClip();
        
        if (clip == null)
        {
            Debug.Log("Attempt to play audio: " + clip.name);
            return;
        }
        
        Debug.Log("Play audio: " + sound_name);
        sfx_source.pitch = Random.Range(0.8f, 1.3f);
        sfx_source.volume = data.max_volume;
        sfx_source.PlayOneShot(clip);
    }
}
