using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthBars : UIComponent
{
    const string playerFillBarReference = "PlayerFillBar";
    const string enemyBarHolderReference = "EnemyBarHolder";
    const string enemyFillBarReference = "EnemyFillBar";
    const string bossFillBarUSSClassName = "bossFillBar";
    const string playerHealthTextReference = "PlayerHealthText";
    const string enemyHealthTextReference = "EnemyHealthText";
    const string playerAttackTextReference = "PlayerAttack";
    const string playerDefenseTextReference = "PlayerDefense";
    const string enemyAttackTextReference = "EnemyAttack";
    const string enemyCountTextReference = "EnemyCount";

    VisualElement playerFillBar;
    VisualElement enemyBarHolder;
    VisualElement enemyFillBar;

    Label playerHealthText;
    Label enemyHealthText;
    Label playerAttackText;
    Label playerDefenseText;
    Label enemyAttackText;

    Label enemyCountText;

    [SerializeField] PlayerStatsSO inCombatPlayerStatsSO;
    [SerializeField] EnemyStats enemyStats;

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
        playerDefenseText = m_UIElement.Query<Label>(name: playerDefenseTextReference);
        enemyAttackText = m_UIElement.Query<Label>(name: enemyAttackTextReference);

        enemyCountText = m_UIElement.Query<Label>(name: enemyCountTextReference);
    }

    public void ChangePlayerHealthUI(object sender, OnChangeHealthEventArgs eventArgs)
    {
        double newHealth = inCombatPlayerStatsSO.CurrentHealth;
        double maxHealth = inCombatPlayerStatsSO.MaxHealth;
        newHealth = newHealth.Clamp(0, maxHealth);

        playerFillBar.style.width = Length.Percent((float)(newHealth * 100 / maxHealth));
        playerHealthText.text = NumberFormater.StringFormatter(newHealth) + "/" + NumberFormater.StringFormatter(maxHealth);
    }

    public void ChangeEnemyHealth(object sender, OnChangeHealthEventArgs eventArgs)
    {
        double newHealth = enemyStats.currentHealth;
        double maxHealth = enemyStats.maxHealth;
        
        enemyFillBar.style.width = Length.Percent((float)(newHealth * 100 / maxHealth));
        enemyHealthText.text = NumberFormater.StringFormatter(newHealth) + "/" + NumberFormater.StringFormatter(maxHealth);
    }

    public void ChangePlayerStats()
    {
        playerAttackText.text = inCombatPlayerStatsSO.Attack.ToString("0");
        playerDefenseText.text = inCombatPlayerStatsSO.Defense.ToString("0");
    }

    public void ChangeEnemyAttack()
    {
        // enemyAttackText.text = enemyStats.attack.ToString("0");
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
        enemyCountText.text = (enemyStats.currentEnemy + 1).ToString("0") + "/" + (enemyStats.totalEnemies + 1).ToString("0");
        if(enemyStats.currentEnemy == enemyStats.totalEnemies)
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
