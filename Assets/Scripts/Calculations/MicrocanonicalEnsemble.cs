using UnityEngine;
using System.IO;
using Grid;
using System.Collections.Generic;
using System.Text;

namespace Calculations
{
    public class MicrocanonicalEnsemble
    {
        private void Load () 
        {
            string path = Path.Combine(Application.persistentDataPath, "test.map");

            using (BinaryReader reader = new BinaryReader(File.OpenRead(path)))
            {
                
            }
        }

        /// <param name="n">Number of runs</param>
        /// <param name="L">Plane length</param>
        /// <param name="a">Disk radius</param>
        public static void RunEnsemble(int n, int L, float a)
        {            
            string path = Path.Combine(Application.persistentDataPath, "nCdfPdf.dat");

            SortedDictionary<int, int> timesClusterOccured = new SortedDictionary<int, int>();
            SortedDictionary<int, int> clusterDistribution = new SortedDictionary<int, int>();
            for (int i = 0; i < n; i++)
            {
                var grid = GridSystem.GridSetup(8443, 14871, L);

                // double eta;
                // double R = Probabilities.PercolationProbabilityGCE(
                //     (double)grid.n, (double)grid.L,
                //     a, grid.unionFind.firstClusterN, out eta);
                // writer.Write(grid.n);
                
                // writer.Write("\t");
                // writer.Write(eta);
                // writer.Write("\t");
                // writer.Write(R);
                // writer.Write("\n");

                // double eta = Utilities.FillingFactor(grid.n, L, a);
                // double lambda = eta * L * L / a;

                // double poisson = Poisson.PoissonDistribution(grid.n, lambda);

                // write ur own class is better maybe

                int frstCluster = grid.unionFind.firstClusterN;
                if (timesClusterOccured.ContainsKey(frstCluster))
                {
                    timesClusterOccured[frstCluster]++;
                    clusterDistribution[frstCluster]++;
                }
                else
                {
                    timesClusterOccured.Add(frstCluster, 1);
                    clusterDistribution.Add(frstCluster, 1);
                } 

                Object.Destroy(grid.gameObject);
                
            }

            // using (
            //    var writer = new StreamWriter(File.Open(path, FileMode.Create), Encoding.UTF8, 65536))
            // {
            //     int cumulativeSum = 0;
            //     foreach (int key in timesClusterOccured.Keys)
            //     {
            //         cumulativeSum += timesClusterOccured[key];
            //         // double P_L = Probabilities.PercolationExistsProbability(cumulativeSum, n);

            //         // double density = clusterDistribution[key] / (double)n;

            //         writer.Write(
            //             $"{key}\t{Probabilities.PercolationExistsProbability(cumulativeSum, n)}\t{clusterDistribution[key] / (double)n}\n");
            //     }
            // }
        }
    }
}