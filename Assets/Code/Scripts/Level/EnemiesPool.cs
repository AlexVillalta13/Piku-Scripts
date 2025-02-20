using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EnemiesPool : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemiesList;

    private void Awake()
    {
        DisableAllEnemies();
    }

    public void DisableAllEnemies()
    {
        foreach (GameObject enemy in enemiesList)
        {
            enemy.SetActive(false);   
        }
    }

    public GameObject GetEnemy()
    {
        int i = Random.Range(0, enemiesList.Count);
        GameObject enemyToGet = enemiesList[i];
        enemiesList.RemoveAt(i);
        enemyToGet.SetActive(true);
        return enemyToGet;
    }

    public void ReturnEnemyToPool(GameObject enemy)
    {
        if (enemy == null)
        {
            return;
        }
        enemy.SetActive(false);
        enemiesList.Add(enemy);
    }
}
