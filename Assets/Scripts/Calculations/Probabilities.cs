using System;

namespace Calculations
{
    public static class Probabilities
    {
        /// <summary> Poisson distribution for a given n term under the sum </summary>
        /// <param name="n"> n when first cluster occurs</param>
        /// <param name="N"> times ensemble ran</param>
        public static double Poisson2ndTerm(
            int n, float L, float a, double eta)
        {
            double lambda = eta * L * L / a;
            return Math.Pow(lambda, n) / Utilities.Factorial(n);
        }

        /// <summary>
        /// Defined with P_L(a, n) (PDF)
        /// </summary>
        /// <param name="n"> number of trials that stop on or BEFORE THE NTH STEP (when cluster appears) </param>
        /// <returns>probability that a wrapping cluster exists in the microcanonical ensemble BEFORE THE NTH STEP</returns>
        public static double PercolationExistsProbability(int n, int allTrials)
        {
            return n / (double)allTrials;
        }

    }
}