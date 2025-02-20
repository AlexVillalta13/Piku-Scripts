using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyHealth : MonoBehaviour
{
    public static EventHandler<OnChangeHealthEventArgs> onChangeEnemyHealth;

    [SerializeField] private EnemyStats stats;

    [SerializeField] private GameEvent playerWinFightEvent;
    [SerializeField] private GameEvent playerWinLevelEvent;
    [SerializeField] private GameEvent onEnemyReceivesDamage;

    public bool DEBUG_InvencibleMode = false;

    private void OnEnable()
    {
        // PlayerAttacks.OnPlayerAttacks += EnemyReceivesDamage;
        MinMaxPlayerAttack.OnPlayerAttacks += EnemyReceivesDamage;
    }

    private void OnDisable()
    {
        // PlayerAttacks.OnPlayerAttacks -= EnemyReceivesDamage;
        MinMaxPlayerAttack.OnPlayerAttacks -= EnemyReceivesDamage;
    }

    private void EnemyReceivesDamage(object sender, MinMaxPlayerAttack.OnPlayerAttacksEventArgs eventArgs)
    {
        double playerDamage = eventArgs.PlayerAttackDamage;
        
        if (stats.currentHealth > 0)
        {
            CheckWinConditions(playerDamage);
        }

        onChangeEnemyHealth?.Invoke(this, new OnChangeHealthEventArgs() { healthDifference = -playerDamage });
    }

    private void CheckWinConditions(double incomingDamage)
    {
        stats.currentHealth -= incomingDamage;
        onEnemyReceivesDamage.Raise(new Empty(), this);
        
        if (stats.currentHealth <= 0)
        {
            if (DEBUG_InvencibleMode == true)
            {
                stats.currentHealth = 1f;
                return;
            }
            
            if (stats.currentEnemy == stats.totalEnemies)
            {
                playerWinLevelEvent.Raise(new Empty(), this);
            }
            else
            {
                playerWinFightEvent.Raise(new Empty(), this);
            }

            stats.currentEnemy += 1;
        }
    }

    public void WinCombatDEBUG()
    {
        if (stats.currentEnemy == stats.totalEnemies)
        {
            playerWinLevelEvent.Raise(new Empty(), this);
        }
        else
        {
            playerWinFightEvent.Raise(new Empty(), this);
        }

        stats.currentEnemy += 1;
    }

    public void WinLevelDEBUG()
    {
        playerWinLevelEvent.Raise(new Empty(), this);
    }
}
