using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ComboNumberUI : UIComponent
{
    [SerializeField] private PlayerStatsSO inCombatPlayerStatsSO;
    [SerializeField] private GameEvent onComboAttackButtonPressed;

    private Label comboNumber;
    private VisualElement comboAttackButton;
    private VisualElement comboMeterBar;

    private const string comboNumberReference = "ComboNumber";
    private const string comboAttackButtonReference = "ComboAttackButton";
    private const string comboMeterBarReference = "ComboMeterBar";

    public override void SetElementsReferences()
    {
        base.SetElementsReferences();

        comboNumber = m_UIElement.Query<Label>(name: comboNumberReference);
        comboAttackButton = m_UIElement.Query<VisualElement>(name: comboAttackButtonReference);
        comboMeterBar = m_UIElement.Query<VisualElement>(name: comboMeterBarReference);
    }

    private void OnEnable()
    {
        comboAttackButton.RegisterCallback<ClickEvent>(ComboAttackButtonPressed);
    }

    private void OnDisable()
    {
        comboAttackButton.UnregisterCallback<ClickEvent>(ComboAttackButtonPressed);
    }

    public void UpdateComboNumber()
    {
        comboNumber.text = "Combo: " + inCombatPlayerStatsSO.comboCount;
        UIAnimator.AnimateWidthPercentage(comboMeterBar, CalculateComboBarPercentage());
        
        comboAttackButton.SetEnabled(inCombatPlayerStatsSO.comboCount >= 5);
    }

    private void ComboAttackButtonPressed(ClickEvent evt)
    {
        if (inCombatPlayerStatsSO.comboCount >= 5)
        {
            onComboAttackButtonPressed?.Raise(new Empty(), this);
        }
    }

    private float CalculateComboBarPercentage()
    {
        return MathF.Min(inCombatPlayerStatsSO.comboCount * 20f, 100f);
    }
    
    public void SetCannotTouchComboButton()
    {
        comboAttackButton.pickingMode = PickingMode.Ignore;
    }
    
    public void SetCanTouchComboButton()
    {
        comboAttackButton.pickingMode = PickingMode.Position;
    }
}
