using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyAttacks : MonoBehaviour
{
    [SerializeField] EnemyStats enemyStats;

    public static EventHandler<OnEnemyAttacksEventArgs> OnEnemyAttacks;

    public class OnEnemyAttacksEventArgs : EventArgs
    {
        public double enemyAttackDamage;
    }

    public void EnemyDoAttack()
    {
        double attackDamage = DoubleUtils.RandomDouble(enemyStats.minAttack, enemyStats.maxAttack);
        OnEnemyAttacks?.Invoke(this, new OnEnemyAttacksEventArgs() { enemyAttackDamage = attackDamage});
    }
}
