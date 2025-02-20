using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Bayat.SaveSystem;

public class ComboTutorialController : MonoBehaviour
{
    private const string comboTutorialCompletionStateReference = "ComboTutorialState";
    private bool comboTutorialCompleted = false;
    
    [SerializeField] private PlayerStatsSO inCombatPlayerStatsSO;
    private UIDocument uiDocument;

    private void Awake()
    {
        LoadComboTutorialCompletionState();
        if (comboTutorialCompleted == true)
        {
            Destroy(gameObject);
            return;
        }
        
        uiDocument = GetComponent<UIDocument>();
        uiDocument.rootVisualElement.style.display = DisplayStyle.None;
        uiDocument.rootVisualElement.pickingMode = PickingMode.Ignore;
    }

    public void CheckIfCanInitializeComboTutorial()
    {
        if(comboTutorialCompleted == true) return;
        
        if (inCombatPlayerStatsSO.comboCount >= 5)
        {
            uiDocument.rootVisualElement.style.display = DisplayStyle.Flex;
        }
        else
        {
            uiDocument.rootVisualElement.style.display = DisplayStyle.None;
        }
    }
    
    public void FinishComboTutorial()
    {
        comboTutorialCompleted = true;
        uiDocument.rootVisualElement.style.display = DisplayStyle.None;
        SaveComboTutorialCompletionState();
        Destroy(gameObject);
    }

    private async void SaveComboTutorialCompletionState()
    {
        await SaveSystemAPI.SaveAsync(comboTutorialCompletionStateReference, comboTutorialCompleted);
    }

    private async void LoadComboTutorialCompletionState()
    {
        if (await SaveSystemAPI.ExistsAsync(comboTutorialCompletionStateReference) == true)
        {
            comboTutorialCompleted = await SaveSystemAPI.LoadAsync<bool>(comboTutorialCompletionStateReference);
        }
    }
}
