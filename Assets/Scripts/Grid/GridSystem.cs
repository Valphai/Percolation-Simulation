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
        private SortedDictionary<int, double> Load(string path) 
        {
            var pdf = new SortedDictionary<int, double>();

            using (
                StreamReader reader =
                    new StreamReader(File.Open(path, FileMode.Open))
            )
            {
                string line = System.String.Empty;
                while ((line = reader.ReadLine()) != null)
                {
                    line.Split("\t", 2);
                    pdf[(int)line[0]] = (double)line[1];
                }

                // denormalize
                foreach (int n in pdf.Keys)
                {
                    pdf[n] *= pdf.Count;
                }
            }
            
            return pdf;
		}
        private double[] LoadR(string path) 
        {
            var R = new List<double>();

            using (
                StreamReader reader =
                    new StreamReader(File.Open(path, FileMode.Open))
            )
            {
                string line = System.String.Empty;
                while ((line = reader.ReadLine()) != null)
                {
                    line.Split("\t", 3);
                    R.Add((double)line[1]);
                }

                return R.ToArray();
            }
		}
        /// <param name="N">Number of runs</param>
        /// <param name="L">Plane length</param>
        /// <param name="a">Disk radius</param>
        /* 
            1. Read data from previous runs (PDF),
            2. Denormalize
            3. Rewrite data including from current run (PDF),
            4. Normalize PDF
            5. Read data from previous runs (Rpdf),
            6. Denormalize
            7. Get new Rpdf
            8. ReWrite Rpdf to file
        */
        public void RunEnsemble(int N, int L)
        {
            string pdfPath = Path.Combine(Application.persistentDataPath, $"PDF_L={L}_a={Metrics.DiskRadius}.dat");
            string RpdfPath = Path.Combine(Application.persistentDataPath, $"Rpdf={L}_a={Metrics.DiskRadius}.dat");
            
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

            // 1. Read data from previous runs (PDF),
            // 2. Denormalize
            var pdf = Load(pdfPath);
            double[] cdf = new double[pdf.Count];

            using (
                var writer = 
                    new StreamWriter(File.Open(pdfPath, FileMode.Create), Encoding.UTF8, 65536))
            {
                // 3. Rewrite data including from current run (PDF),
                foreach (int n in timesClusterOccured.Keys)
                {
                    pdf[n]++;
                }
                
                int i = 0;
                // 4. Normalize PDF & get CDF
                foreach (int n in pdf.Keys)
                {
                    pdf[n] /= pdf.Count;
                    cdf[i++] = pdf[n];
                    
                    writer.Write(
                        $"{n}\t{pdf[n]}\n"
                    );
                }
            }

            // 5. Read data from previous runs (Rpdf),
            // 6. Denormalize?
            double[] Rpdf = LoadR(RpdfPath);

            int first = pdf.Keys.First();
            using (
                var writer = 
                    new StreamWriter(File.Open(RpdfPath, FileMode.Create), Encoding.UTF8, 65536))
            {
                foreach (int n in pdf.Keys)
                {
                    double eta = Utilities.FillingFactor(n, L, Metrics.DiskRadius);

                    // 7. Get new Rpdf
                    double R = Utilities.R_L(
                        first, eta, L, Metrics.DiskRadius, cdf);

                    // 8. ReWrite Rpdf to file
                    writer.Write(
                        $"{n}\t{R}\t{eta}\n"
                    );
                }
            }
        }
    }
}
