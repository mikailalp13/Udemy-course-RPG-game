using System.Runtime.Serialization;
using UnityEngine;
using System.Collections;

public class VFX_AutoController : MonoBehaviour
{
    private SpriteRenderer sr;
    [SerializeField] private bool auto_destroy = true;
    [SerializeField] private float destroy_delay = 1;
    [Space]
    [SerializeField] private bool random_offset = true;
    [SerializeField] private bool random_rotation = true;

    [Header("Fade Effect")]    
    [SerializeField] private bool can_fade;
    [SerializeField] private float fade_speed = 1;


    [Header("Random Rotation")]
    [SerializeField] private float min_rotation = 0;
    [SerializeField] private float max_rotation = 360;


    [Header("Random Position")]
    [SerializeField] private float xMinOffset = -0.3f;
    [SerializeField] private float xMaxOffset = 0.3f;
    [Space]
    [SerializeField] private float yMinOffset = -0.3f;
    [SerializeField] private float yMaxOffset = 0.3f;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        if (can_fade)
            StartCoroutine(FadeCo());
        
        ApplyRandomOffset();
        ApplyRandomRotation();

        if (auto_destroy)
            Destroy(gameObject, destroy_delay);
    }

    private IEnumerator FadeCo()
    {
        Color target_color = Color.white;

        while (target_color.a > 0)
        {
            target_color.a = target_color.a - (fade_speed * Time.deltaTime);
            sr.color = target_color;
            yield return null;
        }

        sr.color = target_color;
    }

    private void ApplyRandomOffset()
    {
        if (random_offset == false)
            return;

        float x_offset = Random.Range(xMinOffset, xMaxOffset);
        float y_offset = Random.Range(yMinOffset, yMaxOffset);

        transform.position += new Vector3(x_offset, y_offset);
    }


    private void ApplyRandomRotation()
    {
        if (random_rotation == false)
            return;

        float z_rotation = Random.Range(min_rotation, max_rotation);
        transform.Rotate(0, 0, z_rotation);
    }
}
