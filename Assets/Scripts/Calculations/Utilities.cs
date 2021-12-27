using System;

namespace Calculations
{
    public static class Utilities
    {
        /// <summary>
        /// Defined with ρ
        /// </summary>
        public static double MeanDensity(double n, double L) => n / (L * L);

        /// <summary>
        /// Defined with η
        /// <param name="n"> number of elements</param>
        /// <param name="L"> grid width slash height </param>
        /// <param name="a"> disk radius </param>
        /// </summary>
        /// <returns> I think this should be the case: 1.127 ≤ η ≤ 1.12875 </returns>
        public static double FillingFactor(double n, double L, float a) => MeanDensity(n, L) * a;

        /// <summary>
        /// Defined with φ
        /// </summary>
        /// <returns>total fraction φ of the plane covered by the objects</returns>
        // public static double TotalArea(int n, int L, float a) => 1 - Mathf.Exp(-FillingFactor(n, L, a));

        public static double OmegaLeft(
            int frstClustIndx, double lambda, 
            double n_hat, int k)
        {
            if (k == 0) return 0;
            if (k == frstClustIndx) return 1;

            return (n_hat - (k - 1)) / lambda * OmegaLeft(
                            frstClustIndx, lambda, n_hat, k - 1);
        }
        public static double OmegaRight(
            int frstClustIndx, double lambda, 
            double n_hat, int k, int len)
        {
            if (k == len) return 0;
            if (k == frstClustIndx) return 1;

            return lambda / (n_hat + k) * OmegaRight(
                        frstClustIndx, lambda, n_hat, k + 1, len);
        }
        public static int Factorial(int n)
        {
            int count = n;
            int result = 1;
    
            while (count >= 1)
            {
                result *= count;
                count--;
            }
            return result;
        }
    }
}