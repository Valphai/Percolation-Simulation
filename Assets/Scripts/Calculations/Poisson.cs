using System;

namespace Calculations
{
    public static class Poisson
    {
        // private static float[] PoissonPointProcess(int L, int min, int max)
        // {
        //     int n = Random.Range(min, max);
        //     float meanDensity = n / (L * L);
        //     // float numOfElems = PoissonDistribution(n, meanDensity);
            
        //     // int[] x = new int[numOfElems];
        //     float[] x = new float[n];
    
        //     for (int i = 0; i < n; i++)
        //     {
        //         x[i] = Mathf.Lerp(0, L * 2, Mathf.InverseLerp(0, 1, PoissonDistribution(i, meanDensity)));
        //     }
    
        //     return x;
        // }
    
        public static double PoissonDistribution(int n, double lambda)
        {
            return Math.Exp(-lambda) * Math.Pow(lambda, n) / Utilities.Factorial(n);
        }
    }
}