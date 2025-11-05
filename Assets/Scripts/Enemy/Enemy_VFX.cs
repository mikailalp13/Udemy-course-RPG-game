using System.Data.SqlTypes;
using System.Runtime.Serialization;
using UnityEngine;

public class Enemy_VFX : Entity_VFX
{
    [Header("Counter Attack Window")]
    [SerializeField] private GameObject attack_alert;

    public void EnableAttackAlert(bool enable)
    {
        if (attack_alert == null)
            return;
            
        attack_alert.SetActive(enable);
    }
}   
