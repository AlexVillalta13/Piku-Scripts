using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class CombatBarUI : UIComponent
{
    // String references
    private const string pointerCombarBarReferfence = "PointerCombatBar";
    private const string enemyBricksElementReference = "EnemyAttacks";
    private const string trapBricksElementReference = "TrapAttacks";
    private const string playerBrickElementReference = "PlayerAttacks";
    private const string enemyUSSClassName = "enemyBrick";
    private const string playerUSSClassName = "playerBrick";
    private const string ignoreBrickWithTouchUSSClassName = "ignoreBrickWithTouch";

    // Visual elements references
    private VisualElement pointerCombatBar;
    private VisualElement enemyBricksElementHolder;
    private VisualElement trapBricksElementHolder;
    private VisualElement playerBrickElementHolder;

    // Lists
    private Dictionary<VisualElement, Brick> bricksInBarDict = new Dictionary<VisualElement, Brick>();
    private List<VisualElement> enemyBricksList = new List<VisualElement>();
    private List<VisualElement> trapBricksList = new List<VisualElement>();
    private List<VisualElement> playerBricksList = new List<VisualElement>();
    private List<VisualElement> bricksInPosition = new List<VisualElement>();

    // States
    [ShowInInspector] private bool inCombat = false;



    [Header("Combat Bar Stats")]
    [SerializeField] private float pointerInitialVelocity = 50f;
    [SerializeField] private float pointerMaxVelocity = 90f;
    [SerializeField] private float pointerInitialAccelerationAmount = 2f;
    [SerializeField] private float pointerCurrentAccelerationAmount = 0f;
    [SerializeField] private float pointerMaxAccelerationAmount = 8f;
    [SerializeField] private float pointerAccelerationIncrementAmount = 1f;
    [SerializeField] private float pointerCurrentVelocity = 0f;
    private float pointerPos;
    private float pointerPercentPosition = 0f;
    [SerializeField] private float extraWidthToTouchBrick = 10f;


    [Header("Touch Event")]
    [SerializeField] private TouchBrickEventsSO touchBrickEventsHolder;

    [Header("SO Data")]
    [SerializeField] private BrickTypesSO brickTypesSO;
    
    public override void SetElementsReferences()
    {
        base.SetElementsReferences();
        pointerCombatBar = m_UIElement.Query<VisualElement>(pointerCombarBarReferfence);
        enemyBricksElementHolder = m_UIElement.Query<VisualElement>(enemyBricksElementReference);
        trapBricksElementHolder = m_UIElement.Query<VisualElement>(trapBricksElementReference);
        playerBrickElementHolder = m_UIElement.Query<VisualElement>(playerBrickElementReference);

        pointerCombatBar.style.left = Length.Percent(pointerPercentPosition);
    }

    public void InitializeBrick(Brick brickScriptToSpawn)
    {
        brickScriptToSpawn.SetupBrick(this, playerBrickElementHolder, enemyBricksElementHolder, trapBricksElementHolder);
        StartCoroutine(WaitOneFrameToPosition(brickScriptToSpawn));
    }
    
    public void InitializeBrick(Brick brickScriptToSpawn, float initialPosition)
    {
        brickScriptToSpawn.SetupBrick(this, playerBrickElementHolder, enemyBricksElementHolder, trapBricksElementHolder);
        StartCoroutine(WaitOneFrameToPosition(brickScriptToSpawn, initialPosition));
    }
    
    private IEnumerator WaitOneFrameToPosition(Brick brickScriptToSpawn)
    {
        yield return new WaitForEndOfFrame();

        brickScriptToSpawn.PositionBrick();
        
        yield return new WaitForEndOfFrame();
        
        UpdateBrickCollections(brickScriptToSpawn);
    }
    
    private IEnumerator WaitOneFrameToPosition(Brick brickScriptToSpawn, float initialPosition)
    {
        yield return new WaitForEndOfFrame();

        brickScriptToSpawn.PositionBrick(initialPosition);
        
        yield return new WaitForEndOfFrame();
        
        UpdateBrickCollections(brickScriptToSpawn);
    }

    private void UpdateBrickCollections(Brick brickScriptToSpawn)
    {
        bricksInBarDict.Add(brickScriptToSpawn.GetBrickElementAttached(), brickScriptToSpawn);
        enemyBricksList = enemyBricksElementHolder.Query<VisualElement>(className: enemyUSSClassName).ToList();
        trapBricksList  = trapBricksElementHolder.Query<VisualElement>(className: enemyUSSClassName).ToList();
        playerBricksList = playerBrickElementHolder.Query<VisualElement>(className: playerUSSClassName).ToList();
    }

    public void MovePointer()
    {
        pointerPercentPosition += Time.deltaTime * pointerCurrentVelocity;
        if (pointerPercentPosition > 100f)
        {
            pointerPercentPosition = 100f;
            pointerCurrentVelocity *= -1f;
        }
        else if (pointerPercentPosition < 0f)
        {
            pointerPercentPosition = 0f;
            pointerCurrentVelocity *= -1f;
        }

        pointerCombatBar.style.left = Length.Percent(pointerPercentPosition);
    }

    public void Touch()
    {
        if(inCombat == true)
        {
            pointerPos = pointerCombatBar.resolvedStyle.left + pointerCombatBar.resolvedStyle.width / 2f;

            enemyBricksList = enemyBricksElementHolder.Query<VisualElement>(className: enemyUSSClassName).ToList();
            trapBricksList  = trapBricksElementHolder.Query<VisualElement>(className: enemyUSSClassName).ToList();
            playerBricksList = playerBrickElementHolder.Query<VisualElement>(className: playerUSSClassName).ToList();
            bricksInPosition.Clear();

            foreach (VisualElement element in enemyBricksList)
            {
                if (pointerPos > element.resolvedStyle.left - extraWidthToTouchBrick && pointerPos < element.resolvedStyle.left + element.resolvedStyle.width + extraWidthToTouchBrick)
                {
                    bricksInPosition.Add(element);
                }
            }
            
            if (bricksInPosition.Count == 0)
            {
                foreach (VisualElement element in trapBricksList)
                {
                    if (pointerPos > element.resolvedStyle.left && pointerPos < element.resolvedStyle.left + element.resolvedStyle.width)
                    {
                        bricksInPosition.Add(element);
                    }
                }
            }

            if (bricksInPosition.Count == 0)
            {
                foreach (VisualElement element in playerBricksList)
                {
                    if (pointerPos > element.resolvedStyle.left - extraWidthToTouchBrick && pointerPos < element.resolvedStyle.left + element.resolvedStyle.width + extraWidthToTouchBrick)
                    {
                        bricksInPosition.Add(element);
                    }
                }
            }

            if (bricksInPosition.Count == 0)
            {
                touchBrickEventsHolder.GetPlayerIsHitEvent().Raise(new Empty(), this);
                return;
            }

            VisualElement brickToBreack = null;
            foreach (VisualElement element in bricksInPosition)
            {
                if(element.ClassListContains(ignoreBrickWithTouchUSSClassName))
                {
                    continue;
                }
                if (element.parent.hierarchy.IndexOf(element) > element.parent.hierarchy.IndexOf(brickToBreack))
                {
                    brickToBreack = element;
                }
            }

            if(brickToBreack != null) 
            {
                bricksInBarDict[brickToBreack].EffectWithTouch();
            }
        }
    }

    public void RemoveBrickFromDict(VisualElement brick)
    {
        bricksInBarDict.Remove(brick);
        playerBricksList = playerBrickElementHolder.Query<VisualElement>(className: playerUSSClassName).ToList();
    }

    public void InCombat()
    {
        inCombat = true;
    }

    public void OutOfCombat()
    {
        inCombat = false;
    }

    public void DeleteAllBricks()
    {
        foreach (KeyValuePair<VisualElement, Brick> element in bricksInBarDict)
        {
            element.Value.RemoveBrickElement();
        }
        bricksInBarDict.Clear();
    }

    public int GetPlayerBricksInBar()
    {
        return playerBricksList.Count;
    }

    public int GetBrickTypeCountInBar(BrickTypeEnum brickType)
    {
        int brickTypeCount = 0;
        foreach (KeyValuePair<VisualElement, Brick> element in bricksInBarDict)
        {
            if (element.Value.GetBrickType() == brickType)
            {
                brickTypeCount++;
            }
        }

        return brickTypeCount;
    }

    public void RestartPointerPosition()
    {
        pointerPercentPosition = 0f;
        pointerCombatBar.style.left = Length.Percent(pointerPercentPosition);
    }

    public float GetPointerPercentPosition()
    {
        return pointerPercentPosition;
    }

    public bool PointerIsCenteredOverBrick()
    {
        pointerPos = pointerCombatBar.resolvedStyle.left + pointerCombatBar.resolvedStyle.width / 2f;
        foreach (VisualElement element in bricksInBarDict.Keys)
        {
            if (pointerPos > element.resolvedStyle.left + element.resolvedStyle.width / 2f - 20f && 
                pointerPos < element.resolvedStyle.left + element.resolvedStyle.width / 2f + 20f)
            {
                return true;
            }
        }
        // foreach (VisualElement element in playerBricksList)
        // {
        //     if (pointerPos > element.resolvedStyle.left + element.resolvedStyle.width / 2f - 20f && 
        //         pointerPos < element.resolvedStyle.left + element.resolvedStyle.width / 2f + 20f)
        //     {
        //         return true;
        //     }
        // }
        return false;
    }

    public void RestartPointerVelocity()
    {
        SetPointerVelocity(pointerInitialVelocity);
    }

    public void RestartPointerAcceleration()
    {
        pointerCurrentAccelerationAmount = pointerInitialAccelerationAmount;
    }

    public void AcceleratePointer()
    {
        SetPointerVelocity(Mathf.Abs(pointerCurrentVelocity) + pointerCurrentAccelerationAmount);

        pointerCurrentAccelerationAmount += pointerAccelerationIncrementAmount;
        if (pointerCurrentAccelerationAmount > pointerMaxAccelerationAmount)
        {
            pointerCurrentAccelerationAmount = pointerMaxAccelerationAmount;
        }
        
        if (Mathf.Abs(pointerCurrentVelocity) > pointerMaxVelocity)
        {
            SetPointerVelocity(pointerMaxVelocity);
        }

    }

    private void SetPointerVelocity(float velocity)
    {
        if (pointerCurrentVelocity >= 0f)
        {
            pointerCurrentVelocity = velocity;
        }
        else
        {
            pointerCurrentVelocity = -velocity;
        }
    }
    
    public float GetPointerVelocity() => pointerCurrentVelocity;

    public void StopPointer()
    {
        pointerCurrentVelocity = 0f;
    }
}
