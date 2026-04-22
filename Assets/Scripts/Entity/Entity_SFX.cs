using UnityEngine;

public class Entity_SFX : MonoBehaviour
{
    private AudioSource audio_source;

    [Header("SFX Names")]
    [SerializeField] private string attack_hit;
    [SerializeField] private string attack_miss;

    [Space]
    [SerializeField] private float sound_distance = 15f;
    [SerializeField] private bool show_gizmos;


    private void Awake()
    {
        audio_source = GetComponentInChildren<AudioSource>();
    }


    public void PlayAttackHit()
    {
        AudioManager.instance.PlaySFX(attack_hit, audio_source, sound_distance);
    }


    public void PlayAttackMiss()
    {
        AudioManager.instance.PlaySFX(attack_miss, audio_source, sound_distance);
    }


    private void OnDrawGizmos()
    {
        if (show_gizmos)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, sound_distance);
        }
    }
}
