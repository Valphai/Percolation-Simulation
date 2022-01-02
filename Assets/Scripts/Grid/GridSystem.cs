// #define DEBUG_MODE
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Text;
using Calculations;
using System.Linq;

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

            if (bins.Length == 0 || !bins[0]) CreateBins();
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
        public void RefreshBins()
        {
            foreach (GridBin bin in bins)
            {
                DestroyImmediate(bin.gameObject);
            }
            CreateBins();
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
        private static GridSystem AssignGrid(int L)
        {
            GameObject g = new GameObject();
            g.transform.position = Vector3.zero;

            g.AddComponent(typeof(GridSystem));
            GridSystem grid = g.GetComponent<GridSystem>();

            grid.L = L;

            return grid;
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
            float diamater = Metrics.Diameter;
           
            for (int i = 0; i < n; i++)
            {
                if (unionFind.FirstClusterOccured) return;

                // unity's random bounds both inclusive causing 1 in ten million bug
                float x = Random.Range(-a, L * diamater - a - .001f);
                float z = Random.Range(-a, L * diamater - a - .001f);
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
        /* 
            this method needs to:
            1. Read data from previous runs (PDF),
            2. Append data from current run (PDF) (while changing existing entries),
            4. Normalize CDF
            5. ReWrite CDF to file
        */
        public void RunEnsemble(int N, int L)
        {
            string path = Path.Combine(Application.persistentDataPath, $"PDF_L={L}_a={Metrics.DiskRadius}.dat");
            string path2 = Path.Combine(Application.persistentDataPath, $"R_L={L}_a={Metrics.DiskRadius}.dat");
            
            // 1. Read data from previous runs (PDF),
            // 2. Append data from current run (PDF) (while changing existing entries),
            SortedDictionary<int, int> timesClusterOccured = new SortedDictionary<int, int>();

            for (int i = 0; i < N; i++)
            {
                Run();

                int frstCluster = unionFind.firstClusterN;
                if (frstCluster != 0)
                {
                    if (timesClusterOccured.ContainsKey(frstCluster))
                        timesClusterOccured[frstCluster]++;
                    else
                        timesClusterOccured.Add(frstCluster, 1);
                }
                unionFind = null;
                
            }
            // 4. Normalize CDF
            // 5. ReWrite CDF to file
            int[] keys = timesClusterOccured.Keys.ToArray();
            int first = timesClusterOccured.Keys.First();
            double[] PDF = new double[keys.Length]; 
            using (
               var writer = new StreamWriter(File.Open(path, FileMode.Create), Encoding.UTF8, 65536))
            {
                writer.Write( // non cumulative CDF == PDF, 
                    "n\tPDF\n"
                );
                int i = 0;
                // key == first cluster
                foreach (int key in timesClusterOccured.Keys)
                {
                    double P_L = Probabilities.PercolationExistsProbability(timesClusterOccured[key], N); // non cumulative
                    PDF[i++] = P_L;
                    // 1 strip PercolationExistsProbability to make it non cumultive
                    // 2 record all 2nd terms from poisson
                    // 3 multiply 1 * 2 by rows up to wanted eta and multiply the product by e^-lambda
                    writer.Write(
                        $"{key}\t{P_L}\n"
                    );
                }
            }
            using (
               var writer = new StreamWriter(File.Open(path2, FileMode.Create), Encoding.UTF8, 65536))
            {
                foreach (int key in timesClusterOccured.Keys)
                {
                    double eta = Utilities.FillingFactor(key, L, Metrics.DiskRadius);

                    double w = Utilities.PoissonWeights(
                        first, eta, L, Metrics.DiskRadius, PDF);

                    writer.Write(
                        $"{key}\t{w}\t{eta}\n"
                    );
                }
            }
        }
    }
}
