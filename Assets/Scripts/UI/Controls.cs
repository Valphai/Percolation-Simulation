using UnityEngine;
using Grid;
using Calculations;
using UnityEngine.Pool;

namespace UI
{
    public class Controls : MonoBehaviour
    {
        public PoolHelper PoolHelper;
        private GridSystem grid;
        private int L = 0;
        private int n = 0;
        private bool doneSetup;
        private ObjectPool<Disk> dPool;

        private void Start()	
        {
            grid = GameObject.FindWithTag("Grid").GetComponent<GridSystem>();
        }
        public void DeactivateBins(bool a)
        {
            if (grid != null)
            {
                foreach (GridBin bin in grid.bins)
                {
                    bin.UiRect.gameObject.SetActive(a);
                }
            }
        }
        public void DeactivateDisks(bool a)
        {
            if (grid != null)
            {
                var uf = grid.unionFind;
                for (int i = 0; i < uf.Disks.Length; i++)
                {
                    uf.Disks[i]?.UiRect.gameObject.SetActive(a);
                }
            }
        }
        public void Run(int n = 10)
        {
            MicrocanonicalEnsemble.RunEnsemble(grid, n, 32, Metrics.DiskRadius, PoolHelper);
        }
        public void Visualize()
        {
            if (!doneSetup)
            {
                dPool = new ObjectPool<Disk>(
                    PoolHelper.CreateDisk, PoolHelper.TakeFromPool, PoolHelper.ReleaseFromPool);
                grid = GridSystem.GridSetup(ref grid, dPool, visualize:true);
                doneSetup = true;
            }
            else
            {
                var uf = grid.unionFind;
                for (int i = 0; i < uf.firstClusterN; i++)
                {
                    uf.Disks[i].Color = Color.yellow;
                }
                grid.ReleasePools();
                grid.CleanBins();
            }
            grid.SetupGrid(Grid.Metrics.SpawnLower, Grid.Metrics.SpawnHigher);
        }
        public void SetL(string text) => L = System.Convert.ToInt32(text);
        public void Setn(string text) => n = System.Convert.ToInt32(text);
    }
}