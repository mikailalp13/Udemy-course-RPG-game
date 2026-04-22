using UnityEngine;

public enum RespawnType 
{
    Enter,
    Exit,
    NoneSpecific, // we'll find closest point, either a checkpoint or the enter
    Portal
}
