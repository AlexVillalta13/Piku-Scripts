using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MinMaxHealthBars : UIComponent
{
    const string playerFillBarReference = "PlayerFillBar";
    const string enemyBarHolderReference = "EnemyBarHolder";
    const string enemyFillBarReference = "EnemyFillBar";
    const string bossFillBarUSSClassName = "bossFillBar";
    const string playerHealthTextReference = "PlayerHealthText";
    const string enemyHealthTextReference = "EnemyHealthText";
    const string playerAttackTextReference = "PlayerAttack";
    const string playerDefenseTextReference = "PlayerDefense";
    const string playerComboPowerTextReference = "PlayerComboPower";
    const string enemyAttackTextReference = "EnemyAttack";
    const string enemyCountTextReference = "EnemyCount";

    private VisualElement playerFillBar;
    private VisualElement enemyBarHolder;
    private VisualElement enemyFillBar;

    private Label playerHealthText;
    private Label enemyHealthText;
    private Label playerAttackText;
    private Label playerComboPower; 
    // private Label playerDefenseText;
    private Label enemyAttackText;

    private Label enemyCountText;

    [SerializeField] private ProceduralLevelSO _proceduralLevelSo;
    [SerializeField] private PlayerStatsSO inCombatPlayerStatsSO;
    [SerializeField] private EnemyStats enemyStats;

    private void OnEnable()
    {
        PlayerHealth.onChangePlayerHealth += ChangePlayerHealthUI;
        EnemyHealth.onChangeEnemyHealth += ChangeEnemyHealth;

        HideEnemyBar();
    }

    private void OnDisable()
    {
        PlayerHealth.onChangePlayerHealth -= ChangePlayerHealthUI;
        EnemyHealth.onChangeEnemyHealth -= ChangeEnemyHealth;
    }

    public override void SetElementsReferences()
    {
        base.SetElementsReferences();
        playerFillBar = m_UIElement.Query<VisualElement>(name: playerFillBarReference);
        enemyBarHolder = m_UIElement.Query<VisualElement>(name: enemyBarHolderReference);
        enemyFillBar = m_UIElement.Query<VisualElement>(name: enemyFillBarReference);

        playerHealthText = m_UIElement.Query<Label>(name: playerHealthTextReference);
        enemyHealthText = m_UIElement.Query<Label>(name: enemyHealthTextReference);

        playerAttackText = m_UIElement.Query<Label>(name: playerAttackTextReference);
        playerComboPower = m_UIElement.Query<Label>(name: playerComboPowerTextReference);
        // playerDefenseText = m_UIElement.Query<Label>(name: playerDefenseTextReference);
        enemyAttackText = m_UIElement.Query<Label>(name: enemyAttackTextReference);

        enemyCountText = m_UIElement.Query<Label>(name: enemyCountTextReference);
    }
    
    private void UpdateHealthBar(double currentHealth, double maxHealth, VisualElement fillBar, Label healthText)
    {
        if (currentHealth < 1f && currentHealth > 0f)
        {
            currentHealth = 1f;
        }

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        UIAnimator.AnimateWidthPercentage(fillBar, (float)(currentHealth * 100 / maxHealth));
        healthText.text = NumberFormater.StringFormatter(currentHealth) + "/" + NumberFormater.StringFormatter(maxHealth);
    }

    private void ChangePlayerHealthUI(object sender, OnChangeHealthEventArgs eventArgs)
    {
        UpdateHealthBar(
            inCombatPlayerStatsSO.CurrentHealth,
            inCombatPlayerStatsSO.MaxHealth,
            playerFillBar,
            playerHealthText);
    }

    private void ChangeEnemyHealth(object sender, OnChangeHealthEventArgs eventArgs)
    {
        UpdateHealthBar(
            enemyStats.currentHealth,
            enemyStats.maxHealth,
            enemyFillBar,
            enemyHealthText);
        if(enemyStats.isBoss == true)
        {
            BossModeOn();
        }
        else
        {
            BossModeOff();
        }
    }

    public void ChangePlayerStats()
    {
        playerAttackText.text = NumberFormater.StringFormatter(inCombatPlayerStatsSO.MinAttack) + "-" + NumberFormater.StringFormatter(inCombatPlayerStatsSO.MaxAttack);
        playerComboPower.text = NumberFormater.StringFormatter(inCombatPlayerStatsSO.ComboPower);
    }
    
    public void ChangeEnemyAttack()
    {
        enemyAttackText.text = NumberFormater.StringFormatter(enemyStats.minAttack) + "-" + NumberFormater.StringFormatter(enemyStats.maxAttack);
    }

    public void ShowEnemyBar()
    {
        enemyBarHolder.style.visibility = Visibility.Visible;
    }

    public void HideEnemyBar()
    {
        enemyBarHolder.style.visibility = Visibility.Hidden;
    }

    public void ChangeEnemyCount()
    {
        enemyCountText.text = (_proceduralLevelSo.GetCurrentEnemy()).ToString("0");
        if(enemyStats.isBoss == true)
        {
            BossModeOn();
        }
        else
        {
            BossModeOff();
        }
    }

    private void BossModeOn()
    {
        enemyFillBar.AddToClassList(bossFillBarUSSClassName);
    }

    private void BossModeOff()
    {
        enemyFillBar.RemoveFromClassList(bossFillBarUSSClassName);
    }
}
