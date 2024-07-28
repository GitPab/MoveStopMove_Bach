using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : Singleton<PlatformManager>
{
    public int numberEnemiesDead;
    public int numberEnemies;

    private List<Transform> platformList;

    private void Awake()
    {
        platformList = new List<Transform>();
    }
    private void Start()
    {
        EnemySpawner.Instance.SpawnEnemies();
        StartCoroutine(CheckForRespawn());
    }

    private IEnumerator CheckForRespawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // wait for 1 second
            if (numberEnemiesDead >= numberEnemies)
            {
                EnemySpawner.Instance.ReSpawn(numberEnemies);
                numberEnemiesDead = 0;
            }
        }
    }
    public void OnEnemyDeath()
    {
        numberEnemiesDead++;
    }
}