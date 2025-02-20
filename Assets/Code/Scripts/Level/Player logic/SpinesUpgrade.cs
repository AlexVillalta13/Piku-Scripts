using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinesUpgrade : UpgradeBehaviour
{
    public void spinesEffect()
    {
        if(HasUpgrade() == true)
        {
            PlayerAttacks.OnPlayerAttacks?.Invoke(this, new PlayerAttacks.OnPlayerAttacksEventArgs() { playerAttackDamage = CalculateSpineDamage() });
        }
    }

    private double CalculateSpineDamage()
    {
        return inCombatPlayerStatsSO.SpinesPercentageDamage / 100f * inCombatPlayerStatsSO.Attack;
    }
}
