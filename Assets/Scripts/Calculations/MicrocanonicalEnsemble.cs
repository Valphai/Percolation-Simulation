using UnityEngine;
using System.IO;
using Grid;

namespace Calculations
{
    public class MicrocanonicalEnsemble
    {
        // private static int[] firstClusterNs;
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
        public static void RunEnsemble(int n, int L, int a)
        {
            int numOfDisks = 9900;
            
            string path = Path.Combine(Application.persistentDataPath, "nEtaR.dat");

            using (
                StreamWriter writer = new StreamWriter(File.Open(path, FileMode.Create)))
            {
                for (int i = 0; i < n; i++)
                {
                    var grid = GridSystem.GridSetup(numOfDisks++, L);

                    double eta;
                    double R = Probabilities.PercolationProbabilityGCE(grid.n, grid.L,
                        a, grid.unionFind.firstClusterN, out eta);
                    
                    writer.Write(grid.n);
                    writer.Write("\t");
                    writer.Write(eta);
                    writer.Write("\t");
                    writer.Write(R);
                    writer.Write("\n");

                    Object.Destroy(grid.gameObject);
                }
            }
        }
    }
}