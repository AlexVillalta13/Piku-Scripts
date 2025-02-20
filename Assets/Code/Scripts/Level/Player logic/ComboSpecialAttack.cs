using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboSpecialAttack : MonoBehaviour
{
    [SerializeField] PlayerStatsSO inCombatPlayerStatsSO;

    [SerializeField] GameEvent onUpdateComboNumber;

    public static Action<Queue<float>> OnComboAttack;
    private Queue<float> chainComboAttacks = new Queue<float>();

    public void ResetComboCount()
    {
        inCombatPlayerStatsSO.comboCount = 0;
        UpdateComboCountUI();
    }

    public void IncreaseComboCount()
    {
        inCombatPlayerStatsSO.comboCount++;
        UpdateComboCountUI();
    }

    public void ComboAttack()
    {
        chainComboAttacks.Clear();
        float attack = inCombatPlayerStatsSO.ComboPower * inCombatPlayerStatsSO.comboCount;
        chainComboAttacks.Enqueue(attack);
        if (inCombatPlayerStatsSO.comboCount >= 10)
        {
            int comboCountMultipleOf5 = inCombatPlayerStatsSO.comboCount / 5;
            int comboCountMultipleOf5Since9 = 9 / 5;
            int numberOfExtraChainAttacks = comboCountMultipleOf5 - comboCountMultipleOf5Since9;

            for (int i = 0; i < numberOfExtraChainAttacks; i++)
            {
                attack = inCombatPlayerStatsSO.comboCount * 4;
                chainComboAttacks.Enqueue(attack);
            }
        }
        
        OnComboAttack?.Invoke(chainComboAttacks);
        ResetComboCount();
    }

    public void UpdateComboCountUI()
    {
        onUpdateComboNumber.Raise(new Empty(), this);
    }
}
