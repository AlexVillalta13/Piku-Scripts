using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActivator : MonoBehaviour
{
    [SerializeField] List<EnemySpawn> spawnList = new List<EnemySpawn>();

    [SerializeField] int currentEnemy = -1;

    [SerializeField] private GameEvent onEndOfSceneReached;
    
    [SerializeField] private ProceduralLevelSO proceduralLevelSO;

    private void Awake()
    {
        SetListOfEnemySpawns();
    }

    private void SetListOfEnemySpawns()
    {
        foreach (Transform child in transform)
        {
            spawnList.Add(child.GetComponent<EnemySpawn>());
        }
    }

    public void SetIfEnemyIsBoss(int enemiesCount)
    {
        for (int i = 0; i < enemiesCount; i++)
        {
            if(i == enemiesCount - 1)
            {
                spawnList[i].SetIfEnemyIsBoss(true);
            }
            else
            {
                spawnList[i].SetIfEnemyIsBoss(false); ;
            }
        }
    }

    private void OnEnable()
    {
        currentEnemy = -1;
    }

    public void SetCurrentEnemy(int currentEnemy)
    {
        this.currentEnemy = currentEnemy - 2;
    }

    // private void OnDisable()
    // {
    //     DeleteInstantiatedEnemies();
    // }
    //
    // private void DeleteInstantiatedEnemies()
    // {
    //     currentEnemy = -1;
    //     foreach (EnemySpawn enemySpawn in spawnList)
    //     {
    //         enemySpawn.DeactivateEnemy();
    //     }
    // }

    public void ActivateNextEnemy()
    {
        if(gameObject.activeInHierarchy == false)
        {
            return;
        }
        currentEnemy++;
        if (proceduralLevelSO.currentEnemy - 1 >= spawnList.Count)
        {
            onEndOfSceneReached?.Raise(new Empty(), this);
        }
        else
        {
            spawnList[proceduralLevelSO.currentEnemy - 1].ActivateEnemy();
        }
    }

    public void DeactivatePreviousEnemy()
    {
        if (gameObject.activeInHierarchy == false)
        {
            return;
        }
        if (proceduralLevelSO.currentEnemy - 2 >= 0)
        {
            spawnList[proceduralLevelSO.currentEnemy - 2].DeactivateEnemy();
        }
    }
}
