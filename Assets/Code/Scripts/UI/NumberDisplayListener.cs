using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Sirenix.OdinInspector;

public class NumberDisplayListener : MonoBehaviour
{
    [SerializeField] private bool isPlayer = false;
    [SerializeField] private Color enemyHitColor = Color.white;
    [SerializeField] private Color enemyGetCriticalHitColor = Color.yellow;
    [SerializeField] private Color playerHitColor = Color.red;
    [SerializeField] private Color playerHealsColor = Color.green;

    private Vector3 numberLocalStartPosition = Vector3.zero;
    [HorizontalGroup("row1", Title = "Random Spawn Points")]
    [SerializeField] private float minX = 0.76f;
    [HorizontalGroup("row1")]
    [SerializeField] private float maxX = 1.25f;
    [HorizontalGroup("row2")]
    [SerializeField] private float minY = 0.6f;
    [HorizontalGroup("row2")]
    [SerializeField] private float maxY = 1.5f;
    [HorizontalGroup("row3")]
    [SerializeField] private float minZ = -0.77f;
    [HorizontalGroup("row3")]
    [SerializeField] private float maxZ = 0f;
    private NumberDisplayPool pool;

    private void Awake()
    {
        pool = FindFirstObjectByType<NumberDisplayPool>();

        if(isPlayer == false)
        {
            numberLocalStartPosition.x *= -1;
        }
    }
    private void OnEnable()
    {
        if(isPlayer)
        {
            PlayerHealth.onChangePlayerHealth += ShowNumber;
        }
        else
        {
            EnemyHealth.onChangeEnemyHealth += ShowNumber;
        }
    }

    private void OnDisable()
    {
        if(isPlayer)
        {
            PlayerHealth.onChangePlayerHealth -= ShowNumber;
        }
        else
        {
            EnemyHealth.onChangeEnemyHealth -= ShowNumber;
        }
    }

    private void ShowNumber(object sender, OnChangeHealthEventArgs eventArgs)
    {
        if (eventArgs.spawnNumberTextMesh == false) { return; }

        TextMeshPro number = pool.NumberMeshPool.Get();
        number.fontSize = 5;
        if (isPlayer == false)
        {
            number.color = enemyHitColor;
        }
        else
        {
            if (eventArgs.healthDifference <= 0f)
            {
                number.color = playerHitColor;
            }
            else if (eventArgs.healthDifference > 0f)
            {
                number.color = playerHealsColor;
            }
            //else if(eventArgs.healthDifference == 0f)
            //{
            //    number.color = enemyHitColor;
            //}
        }

        PositionText(number);
        number.text = NumberFormater.StringFormatter(eventArgs.healthDifference);

        number.transform.SetParent(transform);
    }

    private void PositionText(TextMeshPro number)
    {
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        float randomZ = Random.Range(minZ, maxZ);
        numberLocalStartPosition.x = randomX;
        numberLocalStartPosition.y = randomY;
        numberLocalStartPosition.z = randomZ;
        number.transform.position = transform.TransformPoint(numberLocalStartPosition);
    }

    public void ShowWord(string wordToShow)
    {
        if (isPlayer == true)
        {
            TextMeshPro textMesh = pool.NumberMeshPool.Get();
            textMesh.color = Color.white;

            PositionText(textMesh);

            textMesh.text = wordToShow;
            textMesh.fontSize = 3;
        }
    }
}
