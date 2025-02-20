using UnityEngine;
using Random = System.Random;

public static class DoubleUtils
{
    private static System.Random random = new Random();
    public static double RandomDouble(double min, double max)
    {
        return random.NextDouble() * (max - min) + min;
    }
}
