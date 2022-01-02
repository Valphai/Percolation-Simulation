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
        /// <param name="first"> first n entry in CDF </param>
        /// <param name="vals"> CDF </param>
        /// <returns> Poisson weights convolved with P_L </returns>
        public static double R_L(
            int first, double eta, double L, 
            float a, double[] vals)
        {
            double result = 0;
            double weight = 1;
            double weightSum = 0;
            double lambda = eta * L * L / a;

            // peak index
            int nHat = (int)Math.Floor(lambda);

            // left
            for (int i = nHat; i >= first; i--)
            {
                if (i - first < vals.Length)
                {
                    result += weight * vals[i - first];
                }
                weightSum += weight;
                weight *= i / lambda;
            }

            // right
            weight = 1d;
            for (int i = nHat + 1; i < first + vals.Length; i++)
            {
                weight *= lambda / i;
                double value = (i - first < vals.Length) ? vals[i - first] : 1d;
                result += weight * value;
                weightSum += weight;
            }
            result /= weightSum;
            return result;
        }
    }
}