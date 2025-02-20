using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CheckpointMessageUI : UIComponent
{
    private const string checkPointLabelIdentifier = "CheckPointLabel";
    private Label checkPointLabel;

    public override void SetElementsReferences()
    {
        base.SetElementsReferences();
        checkPointLabel = m_UIElement.Query<Label>(name = checkPointLabelIdentifier);
        ScaleDownElement(checkPointLabel);
    }

    private void OnEnable()
    {
        checkPointLabel.RegisterCallback<TransitionEndEvent>(OnChangeScaleEndEvent);
    }

    private void OnDisable()
    {
        checkPointLabel.UnregisterCallback<TransitionEndEvent>(OnChangeScaleEndEvent);
    }

    public void ShowCheckPointReachedMessage()
    {
        SetDisplayElementFlex();
        ScaleUpUI();
        StartCoroutine(ScaleDownRoutine());
    }

    private IEnumerator ScaleDownRoutine()
    {
        yield return new WaitForSeconds(2f);
        ScaleDownUI();
    }

    protected override void OnScaledDown()
    {
        base.OnScaledDown();
        SetDisplayElementNone();
    }
}
