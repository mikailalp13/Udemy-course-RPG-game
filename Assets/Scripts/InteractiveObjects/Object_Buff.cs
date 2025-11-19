using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class Buff
{
    public StatType type;
    public float value;
}

public class Object_Buff : MonoBehaviour
{
    private SpriteRenderer sr;
    private Entity_Stats stats_to_modify;


    [Header("Buff Details")]
    [SerializeField] private Buff[] buffs; // its an array so that with one buff we can gain multiple stuff
    [SerializeField] private string buff_name;
    [SerializeField] private float buff_duration = 4f;
    [SerializeField] private bool can_be_used = true;



    [Header("Vertical Movement")]
    [SerializeField] private float float_speed = 1f;
    [SerializeField] private float float_range = 0.1f;
    private Vector3  start_position;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        start_position = transform.position;
    }

    private void Update()
    {
        float y_offset = Mathf.Sin(Time.time * float_speed) * float_range; // we used this to make our object move vertically
        transform.position = start_position + new Vector3(0, y_offset);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (can_be_used == false)
            return;

        stats_to_modify = collision.GetComponent<Entity_Stats>();
        StartCoroutine(BuffCo(buff_duration));
    }

    private IEnumerator BuffCo(float duration)
    {
        can_be_used = false;
        sr.color = Color.clear; // we wont be able to see the object. we cant destroy it since its connected to a Coroutine
        ApplyBuff(true);

        yield return new WaitForSeconds(duration);

        ApplyBuff(false);
        Destroy(gameObject);
    }

    private void ApplyBuff(bool apply)
    {
        foreach (var buff in buffs)
        {
            if (apply)
                stats_to_modify.GetStatByType(buff.type).AddModifier(buff.value, buff_name);
            else
                stats_to_modify.GetStatByType(buff.type).RemoveModifier(buff_name);
        }
    }
}
