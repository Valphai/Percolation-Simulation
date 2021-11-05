using UnityEngine;
using Grid;
using Calculations;

namespace UI
{
    public class Controls : MonoBehaviour
    {
        private int L = 0;
        private int n = 0;

        public void DeactivateBins(bool a)
        {
            var g = GameObject.FindWithTag("Grid").GetComponent<GridSystem>();
            if (g != null)
            {
                foreach (GridBin bin in g.bins)
                {
                    bin.UiRect.gameObject.SetActive(a);
                }
            }
        }
        public void DeactivateDisks(bool a)
        {
            var g = GameObject.FindWithTag("Grid").GetComponent<GridSystem>();
            if (g != null)
            {
                foreach (Disk disk in g.unionFind.Disks)
                {
                    disk.UiRect.gameObject.SetActive(a);
                }
            }
        }
        public void Run(int n = 10)
        {
            MicrocanonicalEnsemble.RunEnsemble(n, 32, Metrics.DiskRadius);
        }
        public void Visualize()
        {
            var g = GridSystem.GridSetup(3000, visualize:true);
        }
        public void SetL(string text) => L = System.Convert.ToInt32(text);
        public void Setn(string text) => n = System.Convert.ToInt32(text);
    }
}