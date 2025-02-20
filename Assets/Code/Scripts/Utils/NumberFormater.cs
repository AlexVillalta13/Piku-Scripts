using System;
using UnityEngine;

public class NumberFormater : MonoBehaviour
{
    private static readonly string[] Suffixes = { "", "k", "M", "B", "T", "AA"};

    public static string StringFormatter(double value)
    {
        if (Math.Abs(value) < 1000)
        {
            return value.ToString("0");
        }

        value = value / 1000;
        int suffixIndex = 1;
        while (Math.Abs(value) >= 1000 && suffixIndex < Suffixes.Length - 1)
        {
            value = value / 1000;
            suffixIndex++;
        }

        return value.ToString("0.##") + Suffixes[suffixIndex];
    }
}
