using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneElementsHolder : MonoBehaviour
{
    private EnemyActivator _enemyActivator;
    private PlayerPrefabSpawn _playerPrefabSpawn;

    private void Awake()
    {
        _enemyActivator = GetComponentInChildren<EnemyActivator>();
        _playerPrefabSpawn = GetComponentInChildren<PlayerPrefabSpawn>();
    }

    public void SetupLevel(int enemiesCount)
    {
        _enemyActivator.SetIfEnemyIsBoss(enemiesCount);
    }

    public int GetPlayerSpawnPointsCount()
    {
        return _playerPrefabSpawn.GetPlayerSpawnPointsCount();
    }

    public void SetPlayerSpawnPosition(int enemyCount)
    {
        _playerPrefabSpawn.ActivatePlayerInScene(enemyCount);
        _enemyActivator.SetCurrentEnemy(enemyCount);
    }
}