using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EndLevelUI : UIComponent
{
    private const string LastBeatenEnemyNumberLabelReference = "LastEnemyNumberLabel";
    private const string RecordNumberLabelReference = "RecordNumberLabel";

    private Label lastBeatenEnemyNumberLabel;
    private Label recordNumberLabel;
    
    [SerializeField] private GameEvent onEndLevelUIShowedEvent;
    [SerializeField] private GameEvent returnToMainMenuEvent;
    [SerializeField] private float timeToShowUIAfterDeath = 1.5f;
    [SerializeField] private ProceduralLevelSO proceduralLevelSO;

    public override void SetElementsReferences()
    {
        base.SetElementsReferences();
        lastBeatenEnemyNumberLabel = m_UIElement.Query<Label>(name: LastBeatenEnemyNumberLabelReference);
        recordNumberLabel = m_UIElement.Query<Label>(name: RecordNumberLabelReference);
    }

    private void UpdateEnemiesBeatenText()
    {
        lastBeatenEnemyNumberLabel.text = proceduralLevelSO.currentEnemy.ToString();
        recordNumberLabel.text = proceduralLevelSO.LastCheckPoint.ToString();
    }

    public void StartCoroutineEnableTapToReturn()
    {
        StartCoroutine(EnableTapToReturnToMainMenuCoroutine());
    }

    private IEnumerator EnableTapToReturnToMainMenuCoroutine()
    {
        yield return new WaitForSeconds(timeToShowUIAfterDeath);
        UpdateEnemiesBeatenText();
        SetDisplayElementFlex();
        onEndLevelUIShowedEvent.Raise(new Empty(), this);
        EnableTapToReturnToMainMenu();
    }

    private void EnableTapToReturnToMainMenu()
    {
        m_UIElement.RegisterCallback<ClickEvent>(RaiseReturnToMainMenuEvent);
    }

    private void DisableTapToReturnToMainMenu()
    {
        m_UIElement.UnregisterCallback<ClickEvent>(RaiseReturnToMainMenuEvent);
    }

    private void RaiseReturnToMainMenuEvent(ClickEvent evt)
    {
        DisableTapToReturnToMainMenu();
        returnToMainMenuEvent.Raise(new Empty(), this);
    }
}
