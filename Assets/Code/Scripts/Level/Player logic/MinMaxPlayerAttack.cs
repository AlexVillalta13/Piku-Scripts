using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class MinMaxPlayerAttack : MonoBehaviour
{
    [SerializeField] private PlayerStatsSO playerStatsSo;
    
    public static EventHandler<OnPlayerAttacksEventArgs> OnPlayerAttacks;

    public class OnPlayerAttacksEventArgs : EventArgs
    {
        public double PlayerAttackDamage;
    }
    
    public void CalculatePlayerNormalAttack()
    {
        MakeAttack(GetRandomAttackNumber());
    }

    public void CalculatePlayerCriticalAttack()
    {
        double attackDamage = GetRandomAttackNumber();
        double criticalDamage = (10 * attackDamage / 100) + attackDamage;

        MakeAttack(criticalDamage);
    }

    private void MakeAttack(double attackPower)
    {
        OnPlayerAttacks?.Invoke(this, new OnPlayerAttacksEventArgs() { PlayerAttackDamage = attackPower });
    }

    private double GetRandomAttackNumber()
    {
        return DoubleUtils.RandomDouble(playerStatsSo.MinAttack, playerStatsSo.MaxAttack);
    }
}
