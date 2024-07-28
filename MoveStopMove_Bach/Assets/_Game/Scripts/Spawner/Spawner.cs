using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public virtual Transform SpawnPos()
    {
        return null;
    }
    
    protected virtual void OnSpawn()
    {

    }
}
