using System;
using UnityEngine;

namespace Calculations
{
    public static class Probabilities
    {
        // public static int NumOfTrials;

        public static double PercolationProbabilityGCE(
            int n, int L, int a, 
            int nWhenClusterAppears, int h, int m)
        {

            float lambda = Utilities.FillingFactor(n, L, a) * L * L / a;
            double exp = Mathf.Exp(-lambda);
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

            return (leftPart + rightPart) / omega;
        }
        public static double PercolationProbabilityGCE(
            int n, int L, int a, 
            int nWhenClusterAppears, out double eta)
        {
            eta = Utilities.FillingFactor(n, L, a);

            double lambda = eta * L * L / a;
            double R = Math.Exp(-lambda);
            Debug.Log(eta);
            Debug.Log(lambda);
            Debug.Log(R);

            double sum = 0;
            for (int i = 0; i < nWhenClusterAppears; i++)
            {
                sum += Math.Pow(lambda, i) / Utilities.Factorial(i) * PercolationExistsProbability(
                                                                            i, nWhenClusterAppears);
            }

            return R * sum;
        }
        
        /// <summary>
        /// Defined with P_L(a, n)
        /// </summary>
        /// <param name="n"> number of trials that stop on or BEFORE THE NTH STEP (when cluster appears) </param>
        /// <returns>probability that a wrapping cluster exists in the microcanonical ensemble BEFORE THE NTH STEP</returns>
        public static double PercolationExistsProbability(int n, int nthStep)
        {
            return n / nthStep;//NumOfTrials;
        }

    }
}