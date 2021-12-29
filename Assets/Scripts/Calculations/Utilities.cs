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
        /// <summary> w_n ∝ λ^n/n! </summary>
        /// <param name="first"> first n entry in PDF </param>
        /// <returns> Poisson weights for a given entry </returns>
        public static double PoissonWeights(
            int first, double eta, double L, 
            float a, int frstClust)
        {
            double lambda = eta * L * L / a;

            // peak index
            int n_hat = (int)Math.Floor(lambda);

            double wR = 0;
            double wL = 0;
            double lftW = Utilities.W_Nleft(lambda, ref wR, n_hat, n_hat);
            double rtW = Utilities.W_Nright(lambda, ref wL, n_hat, n_hat + 1);

            return (lftW + rtW) / (wR + wL);
        }
        private static double W_Nleft(
            double lambda, ref double wL,
            double n_hat, int k)
        {
            if (k == 0) return 1;

            wL = W_Nleft(lambda, ref wL, n_hat, k - 1);
            return (n_hat - (k - 1)) / lambda * wL;
        }
        private static double W_Nright(
            double lambda, ref double wR, 
            double n_hat, int k)
        {
            if (k == 0) return 1;
            
            wR = W_Nright(lambda, ref wR, n_hat, k - 1);
            return lambda / (n_hat + k) * wR;
        }
    }
}