using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    [SerializeField] PlayerStatsSO permanentPlayerStatsSO;
    [SerializeField] PlayerStatsSO inCombatPlayerStatsSO;

    [SerializeField] UpgradeInLevelSO upgradesPlayerHasSO;

    [SerializeField] GameEvent onPlayerChangeInCombatStat;

    public void StartLevel()
    {
        upgradesPlayerHasSO.UpgradeList.Clear();

        inCombatPlayerStatsSO.CopyPlayerStatsFrom(permanentPlayerStatsSO);
        
        onPlayerChangeInCombatStat.Raise(new Empty(), this);
        PlayerHealth.onChangePlayerHealth?.Invoke(this, new OnChangeHealthEventArgs() { spawnNumberTextMesh = false});
    }
}
