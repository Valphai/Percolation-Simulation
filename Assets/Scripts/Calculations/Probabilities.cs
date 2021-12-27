using System;
using UnityEngine;

namespace Calculations
{
    public static class Probabilities
    {

        /// <summary> w_n ∝ λ^n/n! </summary>
        /// <param name="h">highest n when cluster appeared (where P_L(a,n) == 1)</param>
        /// <param name="m">smallest n when cluster appeared (where P_L(a,n) != 0)</param>
        /// <returns></returns>
        public static double PercolationProbabilityGCE(
            double eta, double L, float a, 
            int frstClust, int[] timesClustOccured, int[] nCdf)
        {
            double lambda = eta * L * L / a;
            double exp = Math.Exp(-lambda);
            double n_hat = Math.Floor(lambda);

            int i = Array.IndexOf(timesClustOccured, frstClust);

            double omega = 0; // normalization factor

            double leftPart = 0;
            for (int k = i; k > 0; k--)
            {
                double lftOmega = Utilities.OmegaLeft(i, lambda, n_hat, k);
                omega += lftOmega;
                leftPart += lftOmega * nCdf[k];
            }
            double rightPart = 0;
            for (int k = i; k < nCdf.Length; k++)
            {
                double rtOmega = Utilities.OmegaRight(i, lambda, n_hat, k, nCdf.Length);
                omega += rtOmega;
                rightPart += rtOmega * nCdf[k];
            }

            return exp * (leftPart + rightPart) / omega;
        }
        /// <summary> Poisson distribution for a given n term under the sum </summary>
        /// <param name="n"> n when first cluster occurs</param>
        /// <param name="N"> times ensemble ran</param>
        /// <returns></returns>
        public static double Poisson2ndTerm(
            int n, float L, float a, double eta)
        {
            double lambda = eta * L * L / a;
            return Math.Pow(lambda, n) / Utilities.Factorial(n);
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