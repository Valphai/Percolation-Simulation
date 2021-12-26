//#define DEBUG_MODE
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Text;
using Calculations;


namespace Grid
{
    public class GridSystem : MonoBehaviour
    {
        public int L;
        /// <summary>number of times to run the ensemble</summary>
        public int N;
        [SerializeField] 
        public GridBin[] bins { get; private set; }
        [SerializeField]
        public UnionFind unionFind { get; private set; }
        /// <summary>number of disks</summary>
        private int n;
        private GridBin binPrefab;

        private void OnValidate()
        {
            binPrefab = AssetDatabase.LoadAssetAtPath<GridBin>("Assets/Prefabs/Grid/Bin.prefab");

            if (!bins[0]) CreateBins();
            // SetupGrid(Grid.Metrics.SpawnLower, Grid.Metrics.SpawnHigher);
        }
#if DEBUG_MODE
        private void OnDrawGizmosSelected() 
        {
            for (int i = 0; i <= L; i++)
            {
                // vertical
                Gizmos.DrawLine(
                    new Vector3(-Metrics.DiskRadius + i * Metrics.Diameter, 0, -Metrics.DiskRadius),
                    new Vector3(-Metrics.DiskRadius + i * Metrics.Diameter, 0, 
                                    Metrics.Diameter * L - Metrics.DiskRadius)
                );
                // horizontal
                Gizmos.DrawLine(
                    new Vector3(-Metrics.DiskRadius, 0, -Metrics.DiskRadius + i * Metrics.Diameter),
                    new Vector3(Metrics.Diameter * L - Metrics.DiskRadius, 0, 
                                    -Metrics.DiskRadius + i * Metrics.Diameter)
                );
            }
        }
#endif
        public void SetupGrid(int nMin, int nMax)
        {
    		n = UnityEngine.Random.Range(nMin, nMax);
            PopulateGrid();
    	}
        public void CleanBins()
        {
            foreach (GridBin bin in bins)
            {
                bin.CleanDisks();
            }
        }
        public void RemoveBins()
        {
            foreach (GridBin bin in bins)
            {
                Destroy(bin);
            }
        }
        public void Run()
        {
            SetupGrid(Grid.Metrics.SpawnLower, Grid.Metrics.SpawnHigher);
            CleanBins();
        }
        private static GridSystem AssignGrid(int L)
        {
            GameObject g = new GameObject();
            g.transform.position = Vector3.zero;

            g.AddComponent(typeof(GridSystem));
            GridSystem grid = g.GetComponent<GridSystem>();

            grid.L = L;

            return grid;
        }
        private void CreateBins()
        {
            bins = new GridBin[L * L];

            for (int z = 0, i = 0; z < L; z++)
            {
                for (int x = 0; x < L; x++)
                {
                    CreateBin(x, z, i++);
                }
            }
        }
        private void AddDisk(float x, float z, int i, UnionFind uF)
        {
            Vector3 position = new Vector3(
                x,
                Metrics.BinHeight + Metrics.DiskHeight,
                z
            );
            Disk disk = new Disk();
            disk.Position = position;
            
            uF.TickDisk(disk, i);

            disk.Coordinates = Coordinates.FromVectorCoords(position);
    
            // find the bin its on
            GridBin bin = GetBin(disk.Coordinates);
            bin.AddDisk(disk, uF, L);
        }
        private GridBin GetBin(Coordinates coords) 
        {
    		int index = coords.x + coords.z * L;
            try
            {
                return bins[index];
            }
            catch (System.IndexOutOfRangeException)
            {
                Debug.Log(coords.x);
                Debug.Log(coords.z);
                throw;
            }
    	}
        private void PopulateGrid()
        {
            unionFind = new UnionFind(n);
            float a = Metrics.DiskRadius;
            float diamater = a * 2;
           
            for (int i = 0; i < n; i++)
            {
                if (unionFind.FirstClusterOccured) return;

                float x = UnityEngine.Random.Range(-a, L * diamater - a);
                float z = UnityEngine.Random.Range(-a, L * diamater - a);
                AddDisk(x, z, i, unionFind);
            }
        }
        private void CreateBin(int x, int z, int i)
        {
            GridBin bin = bins[i] = Instantiate<GridBin>(binPrefab);
            bin.transform.SetParent(transform, false);
            
            bin.transform.localPosition = new Vector3(
                x * Metrics.Diameter,
                Metrics.BinHeight,
                z * Metrics.Diameter
            );

            bin.Coordinates = Coordinates.FromIntCoords(x, z);
            
            bin.neighbors = new GridBin[8];
            if (x > 0)
            {
                bin.SetNeighbor(Direction.W, bins[i - 1]);
            }
            if (z > 0)
            {
                bin.SetNeighbor(Direction.S, bins[i - L]);
                
                if (x > 0)
                {
                    bin.SetNeighbor(Direction.SW, bins[i - L - 1]);
                }
                else // left
                {
                    bin.SetNeighbor(Direction.SW, bins[i - 1]);
                }
                if (x < L - 1)
                {
                    bin.SetNeighbor(Direction.SE, bins[i - L + 1]);
                }
            }
            if (x == L - 1) // right
            {
                bin.SetNeighbor(Direction.E, bins[i - L + 1]);
                if (z > 0)
                {
                    bin.SetNeighbor(Direction.SE, bins[i - (2 * L) + 1]);
                }
            }
            if (z == L - 1) // upper
            {
                bin.SetNeighbor(Direction.N, bins[i - (L - 1) * L]);
                if (x == 0)
                {
                    bin.SetNeighbor(Direction.NE, bins[i + 1 - (L - 1) * L]);
                    bin.SetNeighbor(Direction.NW, bins[L - 1]);
                }
                else if (x == L - 1)
                {
                    bin.SetNeighbor(Direction.NE, bins[0]);
                    bin.SetNeighbor(Direction.NW, bins[i - 1 - (L - 1) * L]);
                }
                else
                {
                    bin.SetNeighbor(Direction.NE, bins[i + 1 - (L - 1) * L]);
                    bin.SetNeighbor(Direction.NW, bins[i - 1 - (L - 1) * L]);
                }
            }
        }
        /// <param name="N">Number of runs</param>
        /// <param name="L">Plane length</param>
        /// <param name="a">Disk radius</param>
        public void RunEnsemble(int N, int L)
        {
            string path = Path.Combine(Application.persistentDataPath, "data.dat");

            SortedDictionary<int, int> timesClusterOccured = new SortedDictionary<int, int>();
            // SortedDictionary<int, int> clusterDistribution = new SortedDictionary<int, int>();

            for (int i = 0; i < N; i++)
            {
                Run();

                int frstCluster = unionFind.firstClusterN;
                if (timesClusterOccured.ContainsKey(frstCluster))
                {
                    timesClusterOccured[frstCluster]++;
                    // clusterDistribution[frstCluster]++;
                }
                else
                {
                    timesClusterOccured.Add(frstCluster, 1);//s
                    // clusterDistribution.Add(frstCluster, 1);
                }
                
            }

            using (
               var writer = new StreamWriter(File.Open(path, FileMode.Create), Encoding.UTF8, 65536))
            {
                writer.Write(
                    "frst_clust_N\tPDF\tR(eta)\tCDF\teta\n"
                );
                int cumulativeSum = 0;
                // key == first cluster
                foreach (int key in timesClusterOccured.Keys)
                {
                    cumulativeSum += timesClusterOccured[key];
                    double eta = Utilities.FillingFactor(key, L, Metrics.DiskRadius);
                    // double P_L = Probabilities.PercolationExistsProbability(cumulativeSum, n);

                    // double density = clusterDistribution[key] / (double)n;

                    writer.Write(
                        $"{key}\t{Probabilities.PercolationExistsProbability(cumulativeSum, N)}\t{Probabilities.PercolationProbabilityGCE(key, L, Metrics.DiskRadius, eta)}\t{timesClusterOccured[key] / (double)N}\t{eta}\n");
                }
            }
        }
    }
}
