using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using UnityEngine.UIElements;
using Bayat.SaveSystem;
using DG.Tweening;

public class TutorialController : MonoBehaviour
{
    private const string TutorialStateIdentifier = "TutorialState";
    private bool _tutorialCompleted = false;
    
    CombatBarUI _combarBarUI;

    private const string tapScreenLabelReference = "TapScreenLabel";
    
    private UIDocument _uiDocument;
    private Label tutorialMessageLabel;
    
    [ListDrawerSettings(ListElementLabelName = "_phaseNumber")]
    [OnStateUpdate("UpdatePhaseNumber")]
    [SerializeField] private TutorialPhase[] tutorialPhases;
    private int _currentPhaseNumber = 0;

    private void UpdatePhaseNumber()
    {
        int i = 0;
        foreach (TutorialPhase phase in tutorialPhases)
        {
            phase.SetPhaseName($"Phase {i}");
            i++;
        }
    }

    private void Awake()
    {
        LoadTutorialCompletedState();
        if (_tutorialCompleted == true)
        {
            Destroy(gameObject);
            return;
        }
        
        _combarBarUI = FindAnyObjectByType<CombatBarUI>();
        
        _uiDocument = GetComponent<UIDocument>();
        _uiDocument.rootVisualElement.style.display = DisplayStyle.None;
        
        tutorialMessageLabel = _uiDocument.rootVisualElement.Q<Label>(tapScreenLabelReference);
    }

    private void Start()
    {
        if (_tutorialCompleted == false)
        {
            tutorialPhases[0].StartPhase();
        }
    }

    private void Update()
    {

        if (_combarBarUI.PointerIsCenteredOverBrick() && _combarBarUI.GetPointerVelocity() > 0.1f)
        {
            NextTutorialPhase(1);
            NextTutorialPhase(4);
        }

        if (_combarBarUI.GetPointerPercentPosition() < 0.1f)
        {
            NextTutorialPhase(3);
        }
    }

    public void NextTutorialPhase(int phaseNumber)
    {
        if(phaseNumber != _currentPhaseNumber) {return;}
        
        _currentPhaseNumber++;
        tutorialPhases[_currentPhaseNumber].StartPhase();
        
        ShowTutorialMessage();
        
        CheckIfLastTutorialPhase();
    }

    private void ShowTutorialMessage()
    {
        if (tutorialPhases[_currentPhaseNumber].GetTutorialMessage() != "")
        {
            _uiDocument.rootVisualElement.style.display = DisplayStyle.Flex;
            tutorialMessageLabel.text = tutorialPhases[_currentPhaseNumber].GetTutorialMessage();
        }
        else
        {
            _uiDocument.rootVisualElement.style.display = DisplayStyle.None;
        }
    }
    
    private void CheckIfLastTutorialPhase()
    {
        if (tutorialPhases.Length - 1 <= _currentPhaseNumber)
        {
            _tutorialCompleted = true;
            SaveTutorialCompletedState();
            Destroy(gameObject);
        }
    }
    
    private async void SaveTutorialCompletedState()
    {
        await SaveSystemAPI.SaveAsync(TutorialStateIdentifier, _tutorialCompleted);
    }

    private async void LoadTutorialCompletedState()
    {
        if (await SaveSystemAPI.ExistsAsync(TutorialStateIdentifier))
        {
            _tutorialCompleted = await SaveSystemAPI.LoadAsync<bool>(TutorialStateIdentifier);
        }
    }
    
    [System.Serializable]
    public class TutorialPhase
    {
        private string _phaseNumber = "0";
        [HideLabel]
        [SerializeField] private string _phaseName = "Phase";
        [SerializeField] private string tutorialMessage = "";
        [SerializeField] private GameEvent onTutorialPhaseEvent;
        [SerializeField] private UnityEvent onTutorialPhase;

        public void SetPhaseName(string phaseNumber)
        {
            this._phaseNumber = phaseNumber;
        }
        
        public void StartPhase()
        {
            onTutorialPhase?.Invoke();
            onTutorialPhaseEvent?.Raise(new Empty(), this);
        }

        public string GetTutorialMessage()
        {
            return tutorialMessage;
        }
    }
}


