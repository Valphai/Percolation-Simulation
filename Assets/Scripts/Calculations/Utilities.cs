using UnityEngine;

namespace Calculations
{
    public static class Utilities
    {
        /// <summary>
        /// Defined with ρ
        /// </summary>
        /// <param name="n"> number of elements</param>
        /// <param name="L"> grid width slash height </param>
        public static float MeanDensity(int n, int L) => n / (L * L);

        /// <summary>
        /// Defined with η
        /// </summary>
        /// <returns> I think this should be the case: 1.127 ≤ η ≤ 1.12875 </returns>
        public static float FillingFactor(int n, int L) => MeanDensity(n, L) * Grid.Metrics.DiskRadius;

        /// <summary>
        /// Defined with φ
        /// </summary>
        /// <returns>total fraction φ of the plane covered by the objects</returns>
        public static double TotalArea(int n, int L) => 1 - Mathf.Exp(-FillingFactor(n, L));

        public static double OmegaLeft(float lambda, int k)
        {
            if (k == 0) return 1;

            double n_hat = Mathf.Floor(lambda);
            return (n_hat - (k - 1)) / lambda * OmegaLeft(lambda, k - 1);
        }
        public static double OmegaRight(float lambda, int k)
        {
            if (k == 0) return 1;

            double n_hat = Mathf.Floor(lambda);
            return lambda / (n_hat + k) * OmegaRight(lambda, k + 1);
        }
        public static float Factorial(int n)
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