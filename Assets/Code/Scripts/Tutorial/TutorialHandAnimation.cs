using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class TutorialHandAnimation : MonoBehaviour
{
    private const string handSpriteReference = "HandSprite";
    private const string circleSpriteReference = "CircleSprite";
    
    private UIDocument _uiDocument;
    private VisualElement handSprite;
    private Tween handSpriteScaleTween;
    private Vector2 handSpriteScale;
    private VisualElement circleSprite;
    private Vector2 circleSpriteScale;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
    }

    private void Start()
    {
        if(_uiDocument == null) {return;}
        
        handSprite = _uiDocument?.rootVisualElement.Q<VisualElement>(handSpriteReference);
        circleSprite = _uiDocument?.rootVisualElement.Q<VisualElement>(circleSpriteReference);

        AnimateHand();
    }

    private void AnimateHand()
    {
        handSpriteScale = handSprite.style.scale.value.value;
        handSpriteScaleTween = DOTween.To(()=>handSpriteScale,
                x=>
                {
                    handSpriteScale = x;
                    handSprite.style.scale = handSpriteScale;
                }, 
                new Vector2(1.2f,1.2f),
                1f)
            .SetEase(Ease.OutCubic)
            .SetLoops(-1, LoopType.Yoyo)
            .OnStepComplete(AnimateCircleSprite);
    }
    
    private void AnimateCircleSprite()
    {
        if(handSpriteScaleTween.CompletedLoops() % 2 == 1) {return;}
        circleSpriteScale = Vector2.one;
        DOTween.To(()=>circleSpriteScale,
                x=>
                {
                    circleSpriteScale = x;
                    circleSprite.style.scale = circleSpriteScale;
                }, 
                new Vector2(4f,4f),
                0.2f)
            .SetEase(Ease.OutQuad)
            .OnComplete(()=>circleSprite.style.scale = Vector2.one);
    }
}
