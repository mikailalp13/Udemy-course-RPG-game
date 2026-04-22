using UnityEngine;

public class AudioRangeController : MonoBehaviour
{
    private AudioSource source;
    private Transform player;

    private float max_volume;
    [SerializeField] private bool show_gizmos;
    [SerializeField] private float min_distance_to_hear_sounds = 12f;


    private void Start()
    {
        player = Player.instance.transform;
        source = GetComponent<AudioSource>();

        max_volume = source.volume;
    }


    private void Update()
    {
        if (player == null)
            return;
        
        float distance = Vector2.Distance(player.position, transform.position);
        float t = Mathf.Clamp01(1 - (distance / min_distance_to_hear_sounds));

        float target_volume = Mathf.Lerp(0, max_volume, t * t);
        source.volume = Mathf.Lerp(source.volume, target_volume, Time.deltaTime * 3);
    }


    private void OnDrawGizmos()
    {
        if (show_gizmos)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, min_distance_to_hear_sounds);
        }
    }
}
