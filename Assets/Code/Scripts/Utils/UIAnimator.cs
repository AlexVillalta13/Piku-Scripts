using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public static class UIAnimator
{
    public static void AnimateWidthPercentage(VisualElement elementToAnimate, float targetWidthPercentage, float duration = 0.3f)
    {
        float currentWidth = elementToAnimate.style.width.value.value; 
        if (float.IsNaN(currentWidth))
            currentWidth = 0f;

        DOTween.To(
            () => currentWidth,
            x =>
            {
                currentWidth = x;
                elementToAnimate.style.width = new Length(x, LengthUnit.Percent);
            },
            targetWidthPercentage,
            duration
        );
    }
}
