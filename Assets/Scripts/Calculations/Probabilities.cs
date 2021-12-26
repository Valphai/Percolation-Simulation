using System;
using UnityEngine;

namespace Calculations
{
    public static class Probabilities
    {
        // public static int NumOfTrials;

        /// <param name="h">highest n when cluster appeared (where P_L(a,n) == 1)</param>
        /// <param name="m">smallest n when cluster appeared (where P_L(a,n) != 0)</param>
        /// <returns></returns>
        public static double PercolationProbabilityGCE(
            double n, double L, float a, 
            int nWhenClusterAppears, int h, int m)
        {

            double eta = Utilities.FillingFactor(n, L, a);
            double lambda = eta * L * L / a;

            double exp = Math.Exp(-lambda);
            double omega = 0; // normalization factor

            double leftPart = 0;
            for (int k = 0; k < h; k++)
            {
                double lftOmega = Utilities.OmegaLeft(lambda, k);
                omega += lftOmega;
                leftPart += lftOmega * PercolationExistsProbability(
                                                k, nWhenClusterAppears);
            }
            double rightPart = 0;
            for (int k = 0; k < m; k++)
            {
                double rtOmega = Utilities.OmegaRight(lambda, k);
                omega += rtOmega;
                rightPart += rtOmega * PercolationExistsProbability(
                                                k, nWhenClusterAppears);
            }

            return exp * (leftPart + rightPart) / omega;
        }
        /// <param name="n"> n when first cluster occurs</param>
        /// <returns></returns>
        public static double PercolationProbabilityGCE(
            int n, float L, float a, double eta)
        {
            double lambda = eta * L * L / a;
            double R = Math.Exp(-lambda);

            double sum = 0;
            for (int i = 0; i < n; i++)
            {
                sum += Math.Pow(lambda, i) / Utilities.Factorial(i) * 
                    PercolationExistsProbability(i, n);
            }

            return Math.Exp(-lambda) * sum;
        }

        /// <summary>
        /// Defined with P_L(a, n) (CDF)
        /// </summary>
        /// <param name="n"> number of trials that stop on or BEFORE THE NTH STEP (when cluster appears) </param>
        /// <returns>probability that a wrapping cluster exists in the microcanonical ensemble BEFORE THE NTH STEP</returns>
        public static double PercolationExistsProbability(int n, int allTrials)
        {
            return n / (double)allTrials;
        }

    }
}