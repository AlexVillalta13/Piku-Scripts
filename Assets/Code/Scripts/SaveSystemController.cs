using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Bayat.SaveSystem;
using Sirenix.OdinInspector;

public class SaveSystemController : MonoBehaviour
{
    [SerializeField] private ProceduralLevelSO _proceduralLevelSo;
    [SerializeField] private PlayerStatsSO inCombatPlayerStatsSo;
    [SerializeField] private PlayerStatsSO basePlayerStatsSo;

    private void Start()
    {
        basePlayerStatsSo.LoadPlayerStats();
        _proceduralLevelSo.LoadLevelData();
    }

    public void CheckpointReached()
    {
        basePlayerStatsSo.CopyPlayerStatsFrom(inCombatPlayerStatsSo);
        basePlayerStatsSo.SavePlayerStats();
    }

    public void CheckIfCheckPointReached()
    {
        _proceduralLevelSo.CheckIfCanSaveCheckPoint();
    }

    [Button]
    public static async void DeleteAllData()
    {
        await SaveSystemAPI.ClearAsync();
        
#if !UNITY_EDITOR
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
#endif
    }
}
