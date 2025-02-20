using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsUpgrade : MonoBehaviour
{
    [SerializeField] PlayerStatsSO PermanentPlayerStatsSO;
    [SerializeField] PlayerStatsSO inCombatPlayerStatsSO;

    [SerializeField] GameEvent onPlayerChangeInCombatStat;
    
    public void IncreaseMaxHealth()
    {
        inCombatPlayerStatsSO.maxHealthLevel++;
        float amountToIncreaseMaxHealth = inCombatPlayerStatsSO.maxHealthLevel * inCombatPlayerStatsSO.MaxHealthIncreasePerLevel;
        // float amountToIncreaseMaxHealth = Mathf.Round(PermanentPlayerStatsSO.MaxHealth * inCombatPlayerStatsSO.MaxHealthIncreasePercentage / 100);
        
        inCombatPlayerStatsSO.MaxHealth += amountToIncreaseMaxHealth;
        inCombatPlayerStatsSO.CurrentHealth += amountToIncreaseMaxHealth;

        PlayerHealth.onChangePlayerHealth(this, new OnChangeHealthEventArgs() { healthDifference = amountToIncreaseMaxHealth });
    }

    public void Heal()
    {
        // double amountToHeal = inCombatPlayerStatsSO.MaxHealth * inCombatPlayerStatsSO.HealPercentage / 100;
        double amountToHeal = inCombatPlayerStatsSO.MaxHealth;
        inCombatPlayerStatsSO.CurrentHealth += amountToHeal;
        inCombatPlayerStatsSO.CurrentHealth = inCombatPlayerStatsSO.CurrentHealth.Clamp(0, inCombatPlayerStatsSO.MaxHealth);

        PlayerHealth.onChangePlayerHealth(this, new OnChangeHealthEventArgs() { healthDifference = amountToHeal });
    }

    public void DefenseIncrease()
    {
        double defenseIncrease = PermanentPlayerStatsSO.Attack * inCombatPlayerStatsSO.MediumDefenseIncreasePercentage / 100;
        if (defenseIncrease < 2)
        {
            defenseIncrease = 2;
        }
        inCombatPlayerStatsSO.Defense += defenseIncrease;
        onPlayerChangeInCombatStat.Raise(new Empty(), this);
    }

    public void IncreaseAttack()
    {
        double attackIncrease = PermanentPlayerStatsSO.Attack * inCombatPlayerStatsSO.LittleAttackIncreasePercentage / 100;
        if (attackIncrease < 1)
        {
            attackIncrease = 1;
        }
        inCombatPlayerStatsSO.Attack += attackIncrease;

        onPlayerChangeInCombatStat.Raise(new Empty(), this);
    }
    
    public void IncreaseMinAttack()
    {
        float attackIncrement = 0f;
        inCombatPlayerStatsSO.minAttackLevel++;
        if (inCombatPlayerStatsSO.minAttackLevel % 3 == 0)
        {
            attackIncrement = inCombatPlayerStatsSO.MinAttackBigIncrement;
            
            inCombatPlayerStatsSO.MinAttackLittleIncrement += inCombatPlayerStatsSO.ConstToIncreaseMinAttackLittleIncrement;
            inCombatPlayerStatsSO.MinAttackBigIncrement += inCombatPlayerStatsSO.ConstToIncreaseMinAttackBigIncrement;
        }
        else
        {
            attackIncrement = inCombatPlayerStatsSO.MinAttackLittleIncrement;
        }
        inCombatPlayerStatsSO.MinAttack += attackIncrement;
        inCombatPlayerStatsSO.MaxAttack += attackIncrement;

        onPlayerChangeInCombatStat.Raise(new Empty(), this);
    }
    
    public void IncreaseMaxAttack()
    {
        float attackIncrement = 0f;
        inCombatPlayerStatsSO.maxAttackLevel++;
        if (inCombatPlayerStatsSO.maxAttackLevel % 3 == 0)
        {
            attackIncrement = inCombatPlayerStatsSO.MaxAttackBigIncrement;
            
            inCombatPlayerStatsSO.MaxAttackLittleIncrement += inCombatPlayerStatsSO.ConstToIncreaseMaxAttackLittleIncrement;
            inCombatPlayerStatsSO.MaxAttackBigIncrement += inCombatPlayerStatsSO.ConstToIncreaseMaxAttackBigIncrement;
        }
        else
        {
            attackIncrement = inCombatPlayerStatsSO.MaxAttackLittleIncrement;
        }
        inCombatPlayerStatsSO.MaxAttack += attackIncrement;

        onPlayerChangeInCombatStat.Raise(new Empty(), this);
    }

    public void IncreaseComboPower()
    {
        int attackIncrement = 0;
        inCombatPlayerStatsSO.comboPowerLevel++;
        if (inCombatPlayerStatsSO.comboPowerLevel % 3 == 0)
        {
            attackIncrement = inCombatPlayerStatsSO.comboPowerBigIncrement;
            
            inCombatPlayerStatsSO.comboPowerLittleIncrement += inCombatPlayerStatsSO.ConstToIncreaseComboPowerLittleIncrement;
            inCombatPlayerStatsSO.comboPowerBigIncrement += inCombatPlayerStatsSO.ConstToIncreaseComboPowerBigIncrement;
        }
        else
        {
            attackIncrement = inCombatPlayerStatsSO.comboPowerLittleIncrement;
        }
        inCombatPlayerStatsSO.ComboPower += attackIncrement;

        onPlayerChangeInCombatStat.Raise(new Empty(), this);
    }

    public void IncreaseCriticalChance()
    {
        inCombatPlayerStatsSO.CriticalAttackChance += inCombatPlayerStatsSO.CriticalChanceIncrease;
        onPlayerChangeInCombatStat.Raise(new Empty(), this);
    }
}
