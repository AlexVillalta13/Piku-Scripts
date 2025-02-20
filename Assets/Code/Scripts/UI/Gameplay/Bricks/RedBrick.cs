using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


public class RedBrick : Brick
{
    [SerializeField] private Sprite shieldSprite;
    [SerializeField] private Sprite shieldBrokenSprite;
    
    private float MovingBrickPositionInBar;

    private Tween leftMovementTween;
    [SerializeField] private float minTimeToCompletePath = 3f;
    [SerializeField] private float maxTimeToCompletePath = 5f;
    private float timeToCompletePath;
    private float velocity = 0f;
    private float movementTarget = 0f;
    private float distance = 0f;
    [FormerlySerializedAs("easeCurve")] [SerializeField] private Ease leftMovementEaseCurve = Ease.Linear;

    private Tween rightMovementTween;
    
    protected override void SetVisualElementParent(VisualElement playerElementParent, VisualElement enemyElementParent, VisualElement trapElementParent)
    {
        this.m_elementParent = enemyElementParent;
        brickRootElementAttached.AddToClassList(enemyUSSClassName);
    }

    public override void PositionBrick()
    {
        leftMovementTween.Kill();
        rightMovementTween.Kill();
        
        if (hitsToDestroyBrick == 3)
        {
            iconElement.style.backgroundImage = new StyleBackground(shieldSprite);
        }
        
        
        brickRootElementAttached.style.visibility = Visibility.Visible;

        MovingBrickPositionInBar = m_elementParent.resolvedStyle.left + m_elementParent.resolvedStyle.width - brickRootElementAttached.resolvedStyle.width / 2f;
        brickRootElementAttached.style.left = MovingBrickPositionInBar;
      
        timeToCompletePath = Random.Range(minTimeToCompletePath, maxTimeToCompletePath);
        movementTarget = 0f - (brickRootElementAttached.resolvedStyle.width / 2f) - 10f;
        distance = Mathf.Abs(movementTarget - MovingBrickPositionInBar);
        velocity = distance / timeToCompletePath;
        // Debug.Log($"GameObject.name: {gameObject.name}, BrickPosition: {brickRootElementAttached.style.left}, Moving Position: {MovingBrickPositionInBar}, leftTween: {leftMovementTween.IsActive()}, rightTween: {rightMovementTween.IsActive()}");

        leftMovementTween = DOTween.To(() => MovingBrickPositionInBar, x=> MovingBrickPositionInBar = x, 
            movementTarget, 
            timeToCompletePath)
            .SetEase(leftMovementEaseCurve);
        // Debug.Log($"GameObject.name: {gameObject.name}, BrickPosition: {brickRootElementAttached.style.left}, Moving Position: {MovingBrickPositionInBar}, leftTween: {leftMovementTween.IsActive()}, rightTween: {rightMovementTween.IsActive()}");

        
        // Debug.Log(gameObject.name + ": MovingBrickPositionInBar: " + MovingBrickPositionInBar + 
        //           ", brickRootElementAttached.style.left: " + brickRootElementAttached.style.left.value + "." + Environment.NewLine +
        //           "leftMovementTween.position: " + leftMovementTween.position);        
        ScaleUpUI();
    }
    
    private void Update()
    {
        if (brickElement.ClassListContains(scaleUpClass))
        {
            TranslateBrick();
        }
    }

    private void TranslateBrick()
    {
        // Debug.Log($"GameObject.name TranslateBrick(): {gameObject.name}, BrickPosition: {brickRootElementAttached.style.left}, Moving Position: {MovingBrickPositionInBar}, leftTween: {leftMovementTween.IsActive()}, rightTween: {rightMovementTween.IsActive()}");
        brickRootElementAttached.style.left = MovingBrickPositionInBar;
        if(MovingBrickPositionInBar <= 0f - (brickRootElementAttached.resolvedStyle.width / 2f))
        {
            leftMovementTween.Kill();
            brickEventsHolder.GetPlayerIsHitEvent().Raise(new Empty(), this);
            ScaleDownUI();
        }
        else if(MovingBrickPositionInBar > m_elementParent.resolvedStyle.width - brickRootElementAttached.resolvedStyle.width / 2f + 10f)
        {
            rightMovementTween.Kill();
            ScaleDownUI();
        }
    }

    public override void EffectWithTouch()
    {
        base.EffectWithTouch();

        currenHitsToDestroyBrick--;
        if(currenHitsToDestroyBrick == 2)
        {
            brickEventsHolder.GetPlayerBlockEvent().Raise(new Empty(), this);
            iconElement.style.backgroundImage = new StyleBackground(shieldBrokenSprite);
            ScaleDownALittleUI();
        }
        else if(currenHitsToDestroyBrick == 1)
        {
            brickEventsHolder.GetPlayerBlockEvent().Raise(new Empty(), this);
            iconElement.style.backgroundImage = null;
            ScaleDownALittleUI();
        }
        else if (currenHitsToDestroyBrick <= 0)
        {
            leftMovementTween.Kill();

            brickEventsHolder.GetPlayerBlockEvent().Raise(new Empty(), this);
            brickElement.AddToClassList(brickFlashClass);

            ScaleDownUI();
        }
    }

    protected override void OnScaledDown()
    {
        base.OnScaledDown();
        leftMovementTween.Kill();
        rightMovementTween.Kill();
        RemoveBrickElement();
        combatBarUI.RemoveBrickFromDict(brickRootElementAttached);
    }

    public void StopBrickLeftMovement()
    {
        leftMovementTween.Kill();
    }

    public void ResumeBrickMovement(int numberOfComboAttacks)
    {
        if(brickRootElementAttached.style.display == DisplayStyle.None) {return;}
        
        rightMovementTween = DOTween.To(() => MovingBrickPositionInBar, x=> MovingBrickPositionInBar = x,
            MovingBrickPositionInBar + CalculatePushDistance(numberOfComboAttacks), 
            2f)
            .SetEase(Ease.OutCubic)
            .OnComplete(RecalculateLeftMovementTween);
    }
    
    private float CalculatePushDistance(int multiplier)
    {
        return 150 * multiplier;
    }

    private void RecalculateLeftMovementTween()
    {
        if (brickElement.ClassListContains(scaleDownClass))
        {
            return;
        }
        
        MovingBrickPositionInBar = brickRootElementAttached.resolvedStyle.left;
        distance = Mathf.Abs(movementTarget - MovingBrickPositionInBar);
        timeToCompletePath = distance / velocity;
        
        leftMovementTween = DOTween.To(() => MovingBrickPositionInBar, x=> MovingBrickPositionInBar = x, 
            0f - (brickRootElementAttached.resolvedStyle.width / 2f) -10f, 
            timeToCompletePath)
            .SetEase(leftMovementEaseCurve);
    }
}
