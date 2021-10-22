using UnityEngine;

namespace Calculations
{
    public static class Poisson
    {
        public static float[] PoissonPointProcess(int L, int min, int max)
        {
            int n = Random.Range(min, max);
            float meanDensity = n / (L * L);
            // float numOfElems = PossionDistribution(n, meanDensity);
            
            // int[] x = new int[numOfElems];
            float[] x = new float[n];
    
            for (int i = 0; i < n; i++)
            {
                x[i] = Mathf.Lerp(0, L * 2, Mathf.InverseLerp(0, 1, PossionDistribution(i, meanDensity)));
            }
    
            return x;
        }
    
        private static float PossionDistribution(int n, float meanDensity)
        {
            return Mathf.Pow(meanDensity, n) / Factorial(n) * Mathf.Exp(-meanDensity);
        }
    
        private static float Factorial(int n)
        {
            int count = n;
            float result = 1;
    
            while (count >= 1)
            {
                result *= count;
                count--;
            }
            return result;
        }
    }
}