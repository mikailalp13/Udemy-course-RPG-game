using System;
using UnityEngine;
using System.Collections;

public class Object_Buff : MonoBehaviour
{
    private Player_Stats stats_to_modify;


    [Header("Buff Details")]
    [SerializeField] private BuffEffectData[] buffs; // its an array so that with one buff we can gain multiple stuff
    [SerializeField] private string buff_name;
    [SerializeField] private float buff_duration = 4f;


    [Header("Vertical Movement")]
    [SerializeField] private float float_speed = 1f;
    [SerializeField] private float float_range = 0.1f;
    private Vector3  start_position;

    private void Awake()
    {
        start_position = transform.position;
    }

    private void Update()
    {
        float y_offset = Mathf.Sin(Time.time * float_speed) * float_range; // we used this to make our object move vertically
        transform.position = start_position + new Vector3(0, y_offset);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        stats_to_modify = collision.GetComponent<Player_Stats>();

        if (stats_to_modify.CanApplyBuffOf(buff_name))
        {
            stats_to_modify.ApplyBuff(buffs, buff_duration, buff_name);
            Destroy(gameObject);
        }

    }
}
