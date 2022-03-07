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
        public static double FillingFactor(double n, double L, float r)
        {
            double RealL = L * 2 * r;
            return MeanDensity(n, RealL) * S(r);
        }

        public static double S(float r) => Math.PI * r * r;

        /// <param name="first"> first n entry in CDF </param>
        /// <param name="vals"> CDF </param>
        /// <returns> Poisson weights convolved with P_L </returns>
        public static double R_L(
            int first, double eta, double L, 
            float r, double[] vals)
        {
            double RealL = L * 2 * r;
            double result = 0d;
            double weight = 1d;
            double weightSum = 0d;
            double lambda = eta * RealL * RealL / S(r);

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