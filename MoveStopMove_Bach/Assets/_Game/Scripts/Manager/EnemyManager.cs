using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : Singleton<EnemyManager>
{
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private List<Transform> spawnPositions = new List<Transform>();
    [SerializeField] private int numberEnemies;

    private List<Enemy> activeEnemies = new List<Enemy>();
    private int numberEnemiesDead;

    private void Awake()
    {
    }

    private void Start()
    {
        SpawnEnemies(numberEnemies);
        StartCoroutine(CheckForRespawn());
    }

    public void SpawnEnemies(int numberEnemies)
    {
        int enemiesToSpawn = Mathf.Min(numberEnemies, 20 - activeEnemies.Count);
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            SpawnEnemy();
        }
    }


    public void ReSpawn(int numberEnemies)
    {
        int enemiesToSpawn = Mathf.Min(numberEnemies, 20 - activeEnemies.Count);
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        Transform spawnPosition = GetRandomSpawnPosition();
        if (spawnPosition != null)
        {
            Enemy enemy = SimplePool.Spawn<Enemy>(enemyPrefab, GetClosestPointOnNavmesh(spawnPosition.position), spawnPosition.rotation);
            enemy.OnInit();

            // Set final position to a random position within a certain range of the spawn position
            float range = 10f;
            Vector3 randomPosition = spawnPosition.position + new Vector3(Random.Range(-range, range), 0, Random.Range(-range, range));
            enemy.finalPosition = randomPosition;

            activeEnemies.Add(enemy);
        }
    }

    private Transform GetRandomSpawnPosition()
    {
        if (spawnPositions.Count == 0) 
            return null;
        return spawnPositions[Random.Range(0, spawnPositions.Count)];
    }

    private Vector3 GetClosestPointOnNavmesh(Vector3 pos)
    {
        NavMeshHit myNavHit;
        if (NavMesh.SamplePosition(pos, out myNavHit, 100, -1))
        {
            return myNavHit.position;
        }
        return pos;
    }
    private IEnumerator CheckForRespawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // wait for 1 second
            int enemiesToRespawn = Mathf.Min(numberEnemies, 20 - activeEnemies.Count);
            if (activeEnemies.Count < enemiesToRespawn)
            {
                ReSpawn(enemiesToRespawn - activeEnemies.Count);
            }
        }
    }

    public void OnEnemyDeath(Enemy enemy)
    {
        activeEnemies.Remove(enemy);
        numberEnemiesDead++;
    }
}