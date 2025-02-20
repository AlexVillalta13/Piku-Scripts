using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class NumberDisplayAnimation : MonoBehaviour
{
    private NumberDisplayPool pool;
    private TextMeshPro _textMeshPro;
    private Camera _mainCamera;

    private void Awake()
    {
        _textMeshPro = GetComponent<TextMeshPro>();
        _mainCamera = Camera.main;
    }

    public void SetPool(NumberDisplayPool pool)
    {
        this.pool = pool;
    }
    
    private void OnEnable()
    {
        StartCoroutine(DOMoveCoroutine());
    }

    private void LateUpdate()
    {
        transform.LookAt(_mainCamera.transform);
        transform.rotation = Quaternion.LookRotation(transform.position - _mainCamera.transform.position);
    }

    private IEnumerator DOMoveCoroutine()
    {
        //transform.DOShakeScale(0.5f);
        transform.DOPunchScale(new Vector3(2f, 2f, 2f), 0.3f, 10);
        Tween moveTween = transform.DOMoveY(1f, 1f);
        yield return moveTween.WaitForCompletion();
        pool.NumberMeshPool.Release(_textMeshPro);
    }
}
