using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerHealth : MonoBehaviour
{
    public static EventHandler<OnChangeHealthEventArgs> onChangePlayerHealth;

    public bool godMode = false;

    [SerializeField] PlayerStatsSO inCombatPlayerStatsSO;

    [SerializeField] GameEvent onPlayerIsDamage;
    [SerializeField] GameEvent onPlayerIsHitWithoutDamage;
    [SerializeField] GameEvent playerDeathEvent;
    [SerializeField] GameEvent playerDodges;

    double attackIncome;

    private bool hasShield = false;

    private void OnEnable()
    {
        EnemyAttacks.OnEnemyAttacks += PlayerReceiveAttack;
    }

    private void OnDisable()
    {
        EnemyAttacks.OnEnemyAttacks -= PlayerReceiveAttack;
    }

    public void PlayerReceiveAttack(object sender, EnemyAttacks.OnEnemyAttacksEventArgs eventArgs)
    {
        if (PlayerDodgesThisAttack() == true) { return; }

        if (ShieldUpgradeBlocksAttack() == true) { return; }

        CalculateDamageIncome(eventArgs);
        
        CheckWinConditions();
    }

    private bool PlayerDodgesThisAttack()
    {
        if (UnityEngine.Random.Range(1f, 100f) < inCombatPlayerStatsSO.DodgeChance)
        {
            playerDodges.Raise(new Empty(), this);
            return true;
        }
        return false;
    }

    private bool ShieldUpgradeBlocksAttack()
    {
        if (hasShield == true)
        {
            onPlayerIsHitWithoutDamage.Raise(new Empty(), this);
            onChangePlayerHealth(this, new OnChangeHealthEventArgs() { healthDifference = 0f });
            hasShield = false;
            return true;
        }
        return false;
    }

    private void CalculateDamageIncome(EnemyAttacks.OnEnemyAttacksEventArgs eventArgs)
    {
        attackIncome = eventArgs.enemyAttackDamage - inCombatPlayerStatsSO.Defense;
        attackIncome = attackIncome.Clamp(0, eventArgs.enemyAttackDamage);
        inCombatPlayerStatsSO.CurrentHealth -= attackIncome;

        onPlayerIsDamage.Raise(new Empty(), this);

        onChangePlayerHealth(this, new OnChangeHealthEventArgs() { healthDifference = -attackIncome });
    }

    private void CheckWinConditions()
    {
        if (inCombatPlayerStatsSO.CurrentHealth < 0)
        {
            if (godMode == true)
            {
                inCombatPlayerStatsSO.CurrentHealth = 1;
            }
            else
            {
                playerDeathEvent.Raise(new Empty(), this);
            }
        }
    }

    public void ActivateShield()
    {
        hasShield = true;
    }
}