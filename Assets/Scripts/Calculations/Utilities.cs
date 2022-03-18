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
    }
}