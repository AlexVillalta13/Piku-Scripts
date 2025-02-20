using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawn : MonoBehaviour
{
    // [SerializeField] List<GameObject> enemyPrefabsList = new List<GameObject>();
    private EnemiesPool _enemiesPool;
    private GameObject enemyInstance;

    bool enemyIsBoss = false;
    GameObject instantiatedEnemy;

    private void Awake()
    {
        _enemiesPool = FindFirstObjectByType<EnemiesPool>();
    }

    private void OnDisable()
    {
        DeactivateEnemy();
    }

    public void SetIfEnemyIsBoss(bool enemyIsBoss)
    {
        this.enemyIsBoss = enemyIsBoss;
    }

    public void DeactivateEnemy()
    {
        // Destroy(instantiatedEnemy);
        _enemiesPool.ReturnEnemyToPool(enemyInstance);
    }

    // private GameObject SelectRandomEnemy()
    // {
    //     return enemyPrefabsList[Random.Range(0, enemyPrefabsList.Count)];
    // }

    public void ActivateEnemy()
    {
        // instantiatedEnemy = Instantiate(SelectRandomEnemy(), transform.position, transform.rotation);
        enemyInstance = _enemiesPool.GetEnemy();
        enemyInstance.transform.SetPositionAndRotation(transform.position, transform.rotation);

        if (enemyIsBoss == true)
        {
            enemyInstance.transform.localScale = 2f * Vector3.one;
        }
        else
        {
            enemyInstance.transform.localScale = Vector3.one;
        }
    }

    // public void DeactivateEnemy()
    // {
    //     _enemiesPool.ReturnEnemyToPool(enemyInstance);
    // }
}
