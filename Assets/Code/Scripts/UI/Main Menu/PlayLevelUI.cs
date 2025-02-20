using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Bayat.SaveSystem;
using UnityEngine.SceneManagement;

public class PlayLevelUI : UIComponent
{
    const string playButtonReference = "PlayButton";
    const string settingsButtonReference = "SettingsButton";
    const string levelSelectedContainerImageReference = "LevelSelectedContainer";
    const string levelSelectedImageReference = "LevelImage";
    const string levelSelectedNameReference = "LevelName";

    Button playButton;
    Button SettingsButton;
    VisualElement levelImageContainer;
    VisualElement levelImage;
    Label levelSelectedName;

    [SerializeField] UnityEvent openLevelSelector;
    [SerializeField] UnityEvent openSettingsWindow;

    [SerializeField] GameEvent onPlayButtonPressedEvent;

    public override void SetElementsReferences()
    {
        base.SetElementsReferences();

        playButton = m_Root.Query<Button>(name: playButtonReference);
        SettingsButton = m_Root.Query<Button>(name: settingsButtonReference);
        // levelImageContainer = m_Root.Query<VisualElement>(name: levelSelectedContainerImageReference);
        // levelImage =levelImageContainer.Query<VisualElement>(levelSelectedImageReference);
        // levelSelectedName = levelImageContainer.Query<Label>(levelSelectedNameReference);
    }

    private void OnEnable()
    {
        playButton.clicked += PlayButtonPressed;
        SettingsButton.clicked += SettingsButtonPressed;
        // levelImageContainer.RegisterCallback<ClickEvent>(OpenLevelSelector);
        // LevelSelectorUI.onSelectedLevelToPlay += ChangeLevelSelectedImageAndName;
    }
    
    private void OnDisable()
    {
        playButton.clicked -= PlayButtonPressed;
        SettingsButton.clicked -= SettingsButtonPressed;
        // levelImageContainer.UnregisterCallback<ClickEvent>(OpenLevelSelector);
        // LevelSelectorUI.onSelectedLevelToPlay -= ChangeLevelSelectedImageAndName;
    }

    private void ChangeLevelSelectedImageAndName(ILevelData level)
    {
        LevelSO levelSO = level as LevelSO;
        if (levelSO != null)
        {
            levelImage.style.backgroundImage = new StyleBackground(levelSO.LevelIcon);
            levelSelectedName.text = levelSO.name;
        }
    }
    
    private void PlayButtonPressed()
    {
        onPlayButtonPressedEvent.Raise(new Empty(), this);
    }

    private void SettingsButtonPressed()
    {
        openSettingsWindow?.Invoke();
    }

    private void OpenLevelSelector(ClickEvent evt)
    {
        openLevelSelector?.Invoke();
    }

    public void SetFocus(bool state)
    {
        m_UIElement.focusable = state;
    }
}
