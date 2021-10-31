using UnityEngine;
using Grid;
using Calculations;

namespace UI
{
    public class Controls : MonoBehaviour
    {
        private int L = 0;
        private int n = 0;
        [SerializeField] private GridSystem gridSystem;

        public void DeactivateBins(bool a)
        {
            foreach (GridBin bin in gridSystem.bins)
            {
                bin.UiRect.gameObject.SetActive(a);
            }
        }
        public void DeactivateDisks(bool a)
        {
            foreach (Disk disk in UnionFind.Disks)
            {
                disk.UiRect.gameObject.SetActive(a);
            }
        }
        public void Run(int n = 10)
        {
            // if (n <= 0)
            // if (L <= 0)
            MicrocanonicalEnsemble.RunEnsemble(n, 512, Metrics.DiskRadius);
        }
        public void SetL(string text) => L = System.Convert.ToInt32(text);
        public void Setn(string text) => n = System.Convert.ToInt32(text);
    }
}