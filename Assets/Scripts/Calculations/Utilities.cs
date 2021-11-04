using System;
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
        public static double MeanDensity(double n, double L) => n / (L * L);

        /// <summary>
        /// Defined with η
        /// </summary>
        /// <returns> I think this should be the case: 1.127 ≤ η ≤ 1.12875 </returns>
        public static double FillingFactor(double n, double L, float a) => MeanDensity(n, L) * a;

        /// <summary>
        /// Defined with φ
        /// </summary>
        /// <returns>total fraction φ of the plane covered by the objects</returns>
        // public static double TotalArea(int n, int L, float a) => 1 - Mathf.Exp(-FillingFactor(n, L, a));

        public static double OmegaLeft(double lambda, int k)
        {
            if (k == 0) return 1;

            double n_hat = Math.Floor(lambda);
            return (n_hat - (k - 1)) / lambda * OmegaLeft(lambda, k - 1);
        }
        public static double OmegaRight(double lambda, int k)
        {
            if (k == 0) return 1;

            double n_hat = Math.Floor(lambda);
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