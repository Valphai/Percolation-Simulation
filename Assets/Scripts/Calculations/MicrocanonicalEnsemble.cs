using UnityEngine;
using UnityEngine.Pool;
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
        public static void RunEnsemble(GridSystem grid, int n, int L, float a, PoolHelper pH)
        {
            string path = Path.Combine(Application.persistentDataPath, "nCdfPdf.dat");

            SortedDictionary<int, int> timesClusterOccured = new SortedDictionary<int, int>();
            SortedDictionary<int, int> clusterDistribution = new SortedDictionary<int, int>();

            var dPool = new ObjectPool<Disk>(
                pH.CreateDisk, pH.TakeFromPool, pH.ReleaseFromPool);

            grid = GridSystem.GridSetup(ref grid, dPool, L);

            for (int i = 0; i < n; i++)
            {
                grid.SetupGrid(Grid.Metrics.SpawnLower, Grid.Metrics.SpawnHigher);

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
                grid.ReleasePools();
                grid.CleanBins();
                
            }

            using (
               var writer = new StreamWriter(File.Open(path, FileMode.Create), Encoding.UTF8, 65536))
            {
                int cumulativeSum = 0;
                foreach (int key in timesClusterOccured.Keys)
                {
                    cumulativeSum += timesClusterOccured[key];
                    // double P_L = Probabilities.PercolationExistsProbability(cumulativeSum, n);

                    // double density = clusterDistribution[key] / (double)n;

                    writer.Write(
                        $"{key}\t{Probabilities.PercolationExistsProbability(cumulativeSum, n)}\t{clusterDistribution[key] / (double)n}\n");
                }
            }
        }
    }
}