using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "EnemyStatsSO", fileName = "New Enemy Stats SO")]
public class EnemyStats : ScriptableObject
{
    public bool isBoss = false;
    public double maxHealth;
    public double currentHealth;
    // public float attack;
    public double minAttack;
    public double maxAttack;

    public List<CurrencyReward> currencyRewardList = new List<CurrencyReward>();

    public int currentEnemy = 0;
    public int totalEnemies = 0;
}
