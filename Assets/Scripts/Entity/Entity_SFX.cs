using UnityEngine;

public class Entity_SFX : MonoBehaviour
{
    private AudioSource audio_source;

    [Header("SFX Names")]
    [SerializeField] private string attack_hit;
    [SerializeField] private string attack_miss;


    private void Awake()
    {
        audio_source = GetComponentInChildren<AudioSource>();
    }


    public void PlayAttackHit()
    {
        AudioManager.instance.PlaySFX(attack_hit, audio_source);
    }


    public void PlayAttackMiss()
    {
        AudioManager.instance.PlaySFX(attack_miss, audio_source);
    }
}
