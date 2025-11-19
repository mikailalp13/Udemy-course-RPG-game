using System.Collections;
using UnityEngine;

public class Entity_VFX : MonoBehaviour
{
    private SpriteRenderer sr;
    private Entity entity;

    [Header("On Taking Damage VFX")]
    [SerializeField] private Material on_damage_material;
    [SerializeField] private float on_damage_vfx_duration = 0.2f;
    private Material original_material;
    private Coroutine on_damage_vfx_coroutine;


    [Header("On Doing Damage VFX")]
    [SerializeField] private Color hit_vfx_color = Color.white;
    [SerializeField] private GameObject hit_vfx;
    [SerializeField] private GameObject crit_hit_vfx;


    [Header("Element Colors")]
    [SerializeField] private Color chill_vfx = Color.cyan;
    [SerializeField] private Color burn_vfx = Color.red;
    [SerializeField] private Color electrify_vfx = Color.yellow;
    private Color original_hit_vfx_color;


    private void Awake()
    {
        entity = GetComponent<Entity>();
        sr = GetComponentInChildren<SpriteRenderer>();
        original_material = sr.material;
        original_hit_vfx_color = hit_vfx_color;
    }


    public void PlayOnStatusVfx(float duration, ElementType element)
    {
        if (element == ElementType.Ice)
            StartCoroutine(PlayStatusVfxCo(duration, chill_vfx));
        else if (element == ElementType.Fire)
            StartCoroutine(PlayStatusVfxCo(duration, burn_vfx));
        else if (element == ElementType.Lightning)
            StartCoroutine(PlayStatusVfxCo(duration, electrify_vfx));
    }

    public void StopAllVfx()
    {
        StopAllCoroutines();
        sr.color = Color.white;
        sr.material = original_material;
    }


    private IEnumerator PlayStatusVfxCo(float duration, Color effect_color)
    {
        float tick_interval = 0.25f;
        float time_has_passed = 0;

        Color light_color = effect_color * 1.2f;
        Color dark_color = effect_color * 0.8f;

        bool toggle = false;

        while (time_has_passed < duration)
        {
            sr.color = toggle ? light_color : dark_color;
            toggle = !toggle;

            yield return new WaitForSeconds(tick_interval);
            time_has_passed = time_has_passed + tick_interval;
        }

        sr.color = Color.white;
    }

    public void CreateOnHitVfx(Transform target, bool is_crit)
    {
        GameObject hit_prefab = is_crit ? crit_hit_vfx : hit_vfx;
        GameObject vfx = Instantiate(hit_prefab, target.position, Quaternion.identity);
        // clones the object original and returns the clone, the second parameter is the location of that clone, third one is rotation

        vfx.GetComponentInChildren<SpriteRenderer>().color = hit_vfx_color;

        if (entity.facing_dir == -1 && is_crit)
            vfx.transform.Rotate(0, 180, 0);

    }
    
    public void UpdateOnHitColor(ElementType element)
    {
        if (element == ElementType.Ice)
            hit_vfx_color = chill_vfx;
        else if (element == ElementType.None)
            hit_vfx_color = original_hit_vfx_color;
    }

    public void PlayeOnDamageVfx()
    {
        if (on_damage_vfx_coroutine != null)
            StopCoroutine(on_damage_vfx_coroutine);

        on_damage_vfx_coroutine = StartCoroutine(OnDamageVfxCo());
    }

    private IEnumerator OnDamageVfxCo()
    {
        sr.material = on_damage_material;

        yield return new WaitForSeconds(on_damage_vfx_duration);

        sr.material = original_material;
    }
}
