using UnityEngine;
using Grid;

namespace Calculations
{
    public class MicrocanonicalEnsemble
    {
        // private static int[] firstClusterNs;

        private static GridSystem GridSetup()
        {
            GameObject g = new GameObject();
            g.transform.position = Vector3.zero;

            GameObject p = new GameObject();
            p.transform.SetParent(g.transform, false);
            p.AddComponent(typeof(GridMesh));

            g.AddComponent(typeof(GridSystem));
            GridSystem grid = g.GetComponent<GridSystem>();
            return grid;
        }

        public static void RunEnsemble(int n, int L)
        {
            int[] firstClusterNs = new int[n];

            for (int i = 0; i < n; i++)
            {
                GridSystem grid = GridSetup();
                grid.L = L;
                firstClusterNs[i] = grid.unionFind.firstClusterN;
            }
            int lowN = Mathf.Min(firstClusterNs);
            int highN = Mathf.Max(firstClusterNs);

            Probabilities.NumOfTrials = n;
            // double R = Probabilities.PercolationProbabilityGCE(, L, lowN, highN);
        }
    }
}