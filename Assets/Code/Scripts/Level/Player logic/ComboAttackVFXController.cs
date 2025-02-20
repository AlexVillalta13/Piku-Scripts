using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ComboAttackVFXController : MonoBehaviour
{
    [SerializeField] private IntGameEvent onComboAttackFinished;

    [SerializeField] private MMF_Player lightningBallAttackEvent;
    private MMF_Scale scaleLightningBallFeedback;
    [SerializeField] private MMF_Player lightningAttackEvent;

    [SerializeField] private Transform missileExplosion;

    private Queue<float> comboChainAttacks;
    private int numberOfComboAttacks = 0;

    private void Awake()
    {
        scaleLightningBallFeedback = lightningBallAttackEvent.GetFeedbackOfType<MMF_Scale>();
    }

    private void OnEnable()
    {
        ComboSpecialAttack.OnComboAttack += StartComboVFXChainEvents;
    }

    private void OnDisable()
    {
        ComboSpecialAttack.OnComboAttack -= StartComboVFXChainEvents;
    }

    private void StartComboVFXChainEvents(Queue<float> comboChainAttacks)
    {
        numberOfComboAttacks = comboChainAttacks.Count;
        this.comboChainAttacks = comboChainAttacks;

        ScaleMissileWithComboAttack();
        
        lightningBallAttackEvent.PlayFeedbacks();
    }

    private void ScaleMissileWithComboAttack()
    {
        switch (numberOfComboAttacks)
        {
            case >= 6:
                ScaleLightningParticles(2f);
                break;
            case >= 5:
                ScaleLightningParticles(1.75f);
                break;
            case >= 4:
                ScaleLightningParticles(1.5f);
                break;
            case >= 3:
                ScaleLightningParticles(1.25f);
                break;
            case >= 2:
                ScaleLightningParticles(1f);
                break;
            case >= 1:
                ScaleLightningParticles(0.5f);
                break;
        }
    }

    private void ScaleLightningParticles(float scale)
    {
        missileExplosion.localScale = Vector3.one * scale;
        scaleLightningBallFeedback.RemapCurveOne = scale;
    }

    public void SendAttackEvent()
    {
        if(comboChainAttacks.Count <= 0) {return;}
        
        MinMaxPlayerAttack.OnPlayerAttacks?.Invoke(this, 
            new MinMaxPlayerAttack.OnPlayerAttacksEventArgs() {PlayerAttackDamage = comboChainAttacks.Dequeue()});
    }

    public void BeginNextComboAttack()
    {
        if (comboChainAttacks.Count > 0)
        {
            lightningAttackEvent.PlayFeedbacks();
            return;
        }
        
        onComboAttackFinished?.Raise(numberOfComboAttacks, this);
    }
}
