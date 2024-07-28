using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : Spawner
{
    [SerializeField] private Enemy enemyPrfab;

    [SerializeField] protected List<Transform> spawnPos = new List<Transform>();

    public static EnemySpawner Instance { get => instance; }

    private static EnemySpawner instance;
    private int numberSpawn;

    private void Awake()
    {
        instance = this;

        this.numberSpawn = 0;
    }
    public Vector3 GetClosestPointOnNavmesh(Vector3 pos)
    {
        NavMeshHit myNavHit;
        if (NavMesh.SamplePosition(pos, out myNavHit, 100, -1))
        {
            return myNavHit.position;
        }
        return pos;
    }
    public override Transform SpawnPos()
    {
        if (spawnPos.Count < numberSpawn + 1)
        {
            return null;
        }
        return spawnPos[numberSpawn];
    }
    protected override void OnSpawn()
    {
        base.OnSpawn();
        for (int i = 0; i < spawnPos.Count; i++)
        {
            Transform spawnPosition = SpawnPos();
            if (spawnPosition != null)
            {
                Enemy enemyPool = SimplePool.Spawn<Enemy>(enemyPrfab, GetClosestPointOnNavmesh(spawnPosition.position), spawnPosition.rotation);
                enemyPool.OnInit();
                numberSpawn++;
            }
        }
    }

    public void SpawnEnemies()
    {
        this.OnSpawn();
    }

    public void ReSpawn(int numberSpawn)
    {
        for (int i = 0; i < numberSpawn; i++)
        {
            Transform spawnPosition = SpawnPos();
            if (spawnPosition != null)
            {
                Enemy enemyPool = SimplePool.Spawn<Enemy>(enemyPrfab, GetClosestPointOnNavmesh(spawnPosition.position), spawnPosition.rotation);
                enemyPool.OnInit();
            }
        }
    }
}