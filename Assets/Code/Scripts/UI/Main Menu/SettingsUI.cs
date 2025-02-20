using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Bayat.SaveSystem;
using UnityEngine.SceneManagement;

public class SettingsUI : UIComponent
{
    private const string closeWindowReference = "CloseButton";
    private const string clearDataButtonReference = "ClearDataButton";
    private const string confirmationWindowReference = "ClearDataConfirmationUI";
    private const string confirmClearDataButtonReference = "ConfirmClearDataButton";
    private const string dontConfirmClearDataButtonReference = "DontClearDataButton";

    private VisualElement closeWindowButton;
    private Button clearDataButton;
    
    private VisualElement confirmationWindow;
    private Button confirmClearDataButton;
    private Button dontConfirmClearDataButton;

    [SerializeField] private UnityEvent onCloseWindow;

    public override void SetElementsReferences()
    {
        base.SetElementsReferences();
        closeWindowButton = m_UIElement.Query<VisualElement>(name: closeWindowReference);
        clearDataButton = m_UIElement.Query<Button>(name: clearDataButtonReference);
        
        confirmationWindow = m_UIElement.Query<VisualElement>(name: confirmationWindowReference);
        confirmClearDataButton = m_UIElement.Query<Button>(name: confirmClearDataButtonReference);
        dontConfirmClearDataButton = m_UIElement.Query<Button>(name: dontConfirmClearDataButtonReference);
    }

    private void OnEnable()
    {
        closeWindowButton.RegisterCallback<ClickEvent>(CloseWindowButtonClicked);
        clearDataButton.clicked += OpenConfirmationWindow;

        confirmClearDataButton.clicked += ClearData;
        dontConfirmClearDataButton.clicked += CloseConfirmationWindow;
    }

    private void OnDisable()
    {
        closeWindowButton.UnregisterCallback<ClickEvent>(CloseWindowButtonClicked);
        clearDataButton.clicked -= OpenConfirmationWindow;
        
        confirmClearDataButton.clicked -= ClearData;
        dontConfirmClearDataButton.clicked -= CloseConfirmationWindow;
    }

    private void CloseWindowButtonClicked(ClickEvent evt)
    {
        onCloseWindow?.Invoke();
    }

    private void OpenConfirmationWindow()
    {
        confirmationWindow.style.display = DisplayStyle.Flex;
    }

    private async void ClearData()
    {
        CloseConfirmationWindow();
        await SaveSystemAPI.ClearAsync();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void CloseConfirmationWindow()
    {
        confirmationWindow.style.display = DisplayStyle.None;
    }
}
