using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DebugUI : MonoBehaviour
{
    private PlayerHealth healthPlayer;
    private EnemyHealth enemyHealth;

    private UIDocument debugUIDocument;

    private Button openDebugUIButton;
    private Button closeDebugUIButton;
    private VisualElement testOptionsElement;
    private Toggle godModeToggle;
    private Toggle enemyInvencibleModeToggle;
    private Button winLevelButton;
    private Button winCombatButton;

    private void Awake()
    {
        debugUIDocument = GetComponent<UIDocument>();
        healthPlayer = FindAnyObjectByType<PlayerHealth>();
        enemyHealth = FindAnyObjectByType<EnemyHealth>();

        testOptionsElement = debugUIDocument.rootVisualElement.Query<VisualElement>(name: "TestOptionsmenu");
        godModeToggle = debugUIDocument.rootVisualElement.Query<Toggle>(name: "GodModeToggle");
        enemyInvencibleModeToggle = debugUIDocument.rootVisualElement.Query<Toggle>(name: "EnemyInvincibleModeToggle");
        openDebugUIButton = debugUIDocument.rootVisualElement.Query<Button>(name: "OpenTestOptionsButton");
        closeDebugUIButton = debugUIDocument.rootVisualElement.Query<Button>(name: "CloseButton");
        winLevelButton = debugUIDocument.rootVisualElement.Query<Button>(name: "WinLevelButton");
        winCombatButton = debugUIDocument.rootVisualElement.Query<Button>(name: "WinCombatButton");
    }

    private void OnEnable()
    {
        openDebugUIButton.clicked += EnableDebugUI;
        closeDebugUIButton.clicked += DisableTestUI;
        winCombatButton.clicked += WinCombatButtonPressed;

        godModeToggle.RegisterValueChangedCallback(GodModeCallback);
        enemyInvencibleModeToggle.RegisterValueChangedCallback(EnemyInvencibleCallback);
    }

    private void OnDisable()
    {
        openDebugUIButton.clicked -= EnableDebugUI;
        closeDebugUIButton.clicked -= DisableTestUI;
        winLevelButton.clicked -= WinLevelButtonPressed;
        winCombatButton.clicked -= WinCombatButtonPressed;

        godModeToggle.UnregisterValueChangedCallback(GodModeCallback);
        enemyInvencibleModeToggle.UnregisterValueChangedCallback(EnemyInvencibleCallback);
    }

    private void GodModeCallback(ChangeEvent<bool> evt)
    {
        healthPlayer.godMode = evt.newValue;
    }

    private void EnemyInvencibleCallback(ChangeEvent<bool> evt)
    {
        enemyHealth.DEBUG_InvencibleMode = evt.newValue;
    }

    private void WinLevelButtonPressed()
    {
        enemyHealth.WinLevelDEBUG();
    }

    private void WinCombatButtonPressed()
    {
        enemyHealth.WinCombatDEBUG();
    }

    private void EnableDebugUI()
    {
        testOptionsElement.style.display = DisplayStyle.Flex;
        openDebugUIButton.style.display = DisplayStyle.None;
    }

    private void DisableTestUI()
    {
        testOptionsElement.style.display = DisplayStyle.None;
        openDebugUIButton.style.display = DisplayStyle.Flex;
    }
}
