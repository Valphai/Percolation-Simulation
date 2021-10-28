using UnityEngine;
using Grid;

namespace Calculations
{
    public class MicrocanonicalEnsemble
    {
        // private static int[] firstClusterNs;

        private static GridSystem GridSetup(int numOfDisks, int L)
        {
            GameObject g = new GameObject();
            g.transform.position = Vector3.zero;

            GameObject p = new GameObject();
            p.transform.SetParent(g.transform, false);
            p.AddComponent(typeof(GridMesh));

            g.AddComponent(typeof(GridSystem));
            GridSystem grid = g.GetComponent<GridSystem>();

            grid.L = L;
            grid.SetupGrid(numOfDisks);

            return grid;
        }

        public static void RunEnsemble(int n, int L)
        {
            // int[] firstClusterNs = new int[n];
            int numOfDisks = 600;
            for (int i = 0; i < n; i++)
            {
                GridSystem grid = GridSetup(numOfDisks++, L);

                // float eta = 0f;
                double R = Probabilities.PercolationProbabilityGCE(grid.n, grid.L,
                                            grid.unionFind.firstClusterN);

                // firstClusterNs[i] = grid.unionFind.firstClusterN;
            }
            // int lowN = Mathf.Min(firstClusterNs);
            // int highN = Mathf.Max(firstClusterNs);

            // Probabilities.NumOfTrials = grid.n;
            // double R = Probabilities.PercolationProbabilityGCE(, L, lowN, highN);
        }
    }
}