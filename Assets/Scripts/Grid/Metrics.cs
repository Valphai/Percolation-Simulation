using UnityEngine;
    
namespace Grid
{
    public static class Metrics
    {
        public const int DiskRadius = 1;
        public const float DiskHeight = .15f;
        public const float BinHeight = 0f;
        public const float BinLabelHeight = BinHeight + .1f;
        public const float DiskLabelHeight = DiskHeight + .2f;
        public const float BinFontSize = .3f;
        public const float DiskFontSize = .7f;
    
        public static float[] PoissonPointProcess(int width, int height, int min, int max)
        {
            int n = Random.Range(min, max);
            float meanDensity = n / (width * height);
            // float numOfElems = PossionDistribution(n, meanDensity);
            
            // int[] x = new int[numOfElems];
            float[] x = new float[n];
    
            for (int i = 0; i < n; i++)
            {
                x[i] = Mathf.Lerp(0, width * 2, Mathf.InverseLerp(0, 1, PossionDistribution(i, meanDensity)));
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