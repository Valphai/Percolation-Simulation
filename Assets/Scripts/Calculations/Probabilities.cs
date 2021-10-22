using UnityEngine;

namespace Calculations
{
    public static class Probabilities
    {
        public static int NumOfTrials;

        public static double PercolationProbabilityGCE(int n, int L, int h, int m)
        {
            float lambda = Utilities.FillingFactor(n, L) * L * L / Grid.Metrics.DiskRadius;
            double exp = Mathf.Exp(-lambda);
            double omega = 0; // normalization factor

            double leftPart = 0;
            for (int k = 0; k < h; k++)
            {
                double lftOmega = Utilities.OmegaLeft(lambda, k);
                omega += lftOmega;
                leftPart += lftOmega * PercolationExistsProbability(k);
            }
            double rightPart = 0;
            for (int k = 0; k < m; k++)
            {
                double rtOmega = Utilities.OmegaRight(lambda, k);
                omega += rtOmega;
                rightPart += rtOmega * PercolationExistsProbability(k);
            }

            return (leftPart + rightPart) / omega;
        }
        
        /// <summary>
        /// Defined with P_L(a, n)
        /// </summary>
        /// <param name="n"> number of trials that stop on or BEFORE THE NTH STEP (when cluster appears) </param>
        /// <returns>probability that a wrapping cluster exists in the microcanonical ensemble BEFORE THE NTH STEP</returns>
        public static double PercolationExistsProbability(int n)
        {
            return n / NumOfTrials;
        }

    }
}