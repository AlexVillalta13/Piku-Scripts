using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Bayat.SaveSystem;

[CreateAssetMenu(fileName = "Procedural Level")]
public class ProceduralLevelSO : ScriptableObject, ILevelData
{
    private const string saveSystemIdentifier = "ProceduralLevelCurrentEnemy";
    [SerializeField] private GameEvent onCheckPointReachedEvent;
    [SerializeField] private SceneEnum environment = SceneEnum.AlpineWoods;
    public SceneEnum GetEnvironment() { return environment; }

    [SerializeField] private int initialHealth = 40;

    [SerializeField] private int minAttack = 3;
    // [SerializeField] private int maxAttack = 7;

    [FormerlySerializedAs("bricksProbabilities")] [SerializeField] private List<BrickProbability> enemyBricksProbabilities = new List<BrickProbability>();
    
    public int currentEnemy = 1;
    [SerializeField] private int lastCheckPoint = 1;
    public int LastCheckPoint => lastCheckPoint;
    public EnemyData enemy;

    public void SetCurrentEnemyToCheckPoint()
    {
        currentEnemy = lastCheckPoint;
        UpdateEnemyData();
    }

    public void NextEnemy()
    {
        currentEnemy++;
        UpdateEnemyData();
        
        // Debug to save every enemy
        // Debug.Log("Save event");
        // lastCheckPoint = currentEnemy;
        // onCheckPointReachedEvent?.Raise(new Empty(), this);
        // SaveLevelData();
        // CheckIfCanSaveCheckPoint();
    }

    public void CheckIfCanSaveCheckPoint()
    {
        if (lastCheckPoint != currentEnemy && currentEnemy % 10 == 1 && currentEnemy != 1)
        {
            lastCheckPoint = currentEnemy;
            onCheckPointReachedEvent?.Raise(new Empty(), this);
            SaveLevelData();
        }
    }

    public EnemyData GetEnemy(int enemyNumber)
    {
        return enemy;
    }

    public int GetCurrentEnemy()
    {
        return currentEnemy;
    }

    private void UpdateEnemyData()
    {
        SetEnemyHealth();
        SetEnemyAttack();
        SetEnemyBricksProbabilities();
    }

    private void SetEnemyHealth()
    {
        int maxHealthToIncrease = initialHealth;
        
        for(int i = 1; i <= currentEnemy; i++)
        {
            if (1 < i && i <= 10)
            {
                maxHealthToIncrease += 16;
            }
            else if (i % 10 == 1 && i > 10)
            {
                maxHealthToIncrease += 109;
            }
            else if (i >= 11)
            {
                maxHealthToIncrease += 41;
            }
        }

        if (currentEnemy % 10 == 0)
        {
            maxHealthToIncrease *= 2;
            enemy.IsBoss = true;
        }
        else
        {
            enemy.IsBoss = false;
        }
        
        enemy.Health = maxHealthToIncrease;
    }

    private void SetEnemyAttack()
    {
        int increment = 2; 
        int totalAttack = minAttack;
        
        for (int i = 2; i <= currentEnemy; i++) 
        {
            if (i > 10)
            {
                increment = 4 + (i - 11) / 10;
            }
            totalAttack += increment;
        }

        enemy.minAttack = totalAttack;
        enemy.maxAttack = enemy.minAttack * 2f + 1f;
    }

    private void SetEnemyBricksProbabilities()
    {
        foreach (BrickProbability brickProbabilityToCompare in enemyBricksProbabilities)
        {
            foreach (BrickProbability enemyCurrentBrickProbability in enemy.EnemyBricks)
            {
                if(enemyCurrentBrickProbability.BrickType == brickProbabilityToCompare.BrickType)
                {
                    if (currentEnemy >= brickProbabilityToCompare.LevelToUnlock)
                    {
                        enemyCurrentBrickProbability.Probability = brickProbabilityToCompare.Probability;
                    }
                    else
                    {
                        enemyCurrentBrickProbability.Probability = 0f;
                    }
                    break;
                }
            }
        }
    }
    
    public int GetTotalEnemiesCount()
    {
        return 0;
    }

    private async void SaveLevelData()
    {
        await SaveSystemAPI.SaveAsync(saveSystemIdentifier, lastCheckPoint);
    }

    public async void LoadLevelData()
    {
        if (await SaveSystemAPI.ExistsAsync(saveSystemIdentifier) == false)
        {
            lastCheckPoint = 1;
        }
        else
        {
            lastCheckPoint = await SaveSystemAPI.LoadAsync<int>(saveSystemIdentifier);
        }
    }
}
