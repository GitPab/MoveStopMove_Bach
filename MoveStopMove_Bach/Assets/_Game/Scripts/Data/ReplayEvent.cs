using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ReplayEvent
{
    public ReplayEventType type;
    public float timestamp;
    public object data;
}

public enum ReplayEventType
{
    PlayerMovement,
    EnemyDeath,
    //...
} 
