using TMPro;
using UnityEditor;
using UnityEngine;


namespace Grid
{
    public class GridSystem : MonoBehaviour
    {
        public int L;
        /// <summary>number of disks</summary>
        public int n { get; private set; }
        public GridBin[] bins { get; private set; }
        public UnionFind unionFind { get; private set; }
        private int minNRange, maxNRange;
        private bool visualize;
        private GridBin binPrefab;
        private Disk diskPrefab;
        private TextMeshProUGUI labelPrefab;
        private GridMesh gridMesh;
        private Canvas gridCanvas;
        // private ObjectPool<Disk> pool;

        private void Awake()
        {
            gridCanvas = GetComponentInChildren<Canvas>();
            gridMesh = GetComponentInChildren<GridMesh>();
        }
        public void SetupGrid()
        {
            CreateBins();
    		if (visualize)
                gridMesh.Triangulate(bins);
                
            PopulateGrid();
    	}
        public static GridSystem GridSetup(int numOfDisks = 600, int L = 40, bool visualize = false)
        {
            var g = AssignGrid(L, visualize);
           
            g.n = numOfDisks;

            g.SetupGrid();
            
            return g;
        }
        public static GridSystem GridSetup(int nMin, int nMax, int L = 40, bool visualize = false)
        {
            var g = AssignGrid(L, visualize);

            g.n = UnityEngine.Random.Range(nMin, nMax);

            g.SetupGrid();
            
            return g;
        }
        private static GridSystem AssignGrid(int L, bool visualize)
        {
            GameObject g = new GameObject();
            g.transform.position = Vector3.zero;

            if (visualize)
            {
                GameObject p = new GameObject();
                p.transform.SetParent(g.transform, false);
                p.AddComponent(typeof(GridMesh));
    
                GameObject c = new GameObject();
                c.transform.SetParent(g.transform, false);
                c.AddComponent(typeof(Canvas));
            }

            g.AddComponent(typeof(GridSystem));
            GridSystem grid = g.GetComponent<GridSystem>();

            grid.L = L;
            grid.visualize = visualize;

            grid.binPrefab = AssetDatabase.LoadAssetAtPath<GridBin>("Assets/Prefabs/Grid/Bin.prefab");
            grid.diskPrefab = AssetDatabase.LoadAssetAtPath<Disk>("Assets/Prefabs/Grid/Disk.prefab");
            grid.labelPrefab = AssetDatabase.LoadAssetAtPath<TextMeshProUGUI>("Assets/Prefabs/Grid/Label.prefab");

            return grid;
        }
        private void OnDestroy()
        {
            Resources.UnloadUnusedAssets();
            Destroy(gameObject);
        }

        private void AddDisk(float x, float z, int i, UnionFind unionFind)
        {
            Vector3 position = new Vector3(
                x,
                Metrics.BinHeight + Metrics.DiskHeight,
                z
            );
    
            Disk disk = Instantiate<Disk>(diskPrefab);
            // var disk = pool.Get();

            unionFind.TickDisk(disk, i);
            
            disk.Position = position;
            disk.transform.SetParent(transform, true);

            disk.Coordinates = Coordinates.FromVectorCoords(position);
    
            // find bin its on
            GridBin bin = GetBin(disk.Coordinates);
            bin.AddDisk(disk, unionFind, L);
    
            if (visualize)
            {
                SetLabel(position, disk.Coordinates, Color.blue, out disk.UiRect,
                        Metrics.DiskFontSize, Metrics.DiskLabelHeight, Metrics.DiskRadius);
            }
        }
        private GridBin GetBin(Coordinates coords) 
        {    
    		int index = coords.x + coords.z * L;
    		return bins[index];
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
        private void PopulateGrid()
        {
            unionFind = new UnionFind(n, visualize);
            float a = Metrics.DiskRadius;
            float diamater = Metrics.DiskRadius * 2;
           
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
            float diameter = Metrics.DiskRadius * 2;
            Vector3 position = new Vector3(
                x * diameter,
                Metrics.BinHeight,
                z * diameter
            );
    
            GridBin bin = bins[i] = Instantiate<GridBin>(binPrefab);
            bin.transform.SetParent(gridMesh.transform, false);
            bin.transform.localPosition = position;
    
            bin.Coordinates = Coordinates.FromIntCoords(x, z);
    
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
                if (x < L - 1)
                {
                    bin.SetNeighbor(Direction.SE, bins[i - L + 1]);
                }
            }
    
            if (visualize)
            {
                SetLabel(position, bin.Coordinates, Color.white, out bin.UiRect,
                        Metrics.BinFontSize, Metrics.BinLabelHeight, diameter);
            }
        }
    
        private void SetLabel(
            Vector3 position, Coordinates coords, Color c, out RectTransform r,
            float fontSize, float height, float scale)
        {
            TextMeshProUGUI label = Instantiate<TextMeshProUGUI>(labelPrefab);
            label.rectTransform.SetParent(gridCanvas.transform, false);
            label.rectTransform.anchoredPosition =
                new Vector3(position.x, position.z);
            label.rectTransform.localPosition += Vector3.back * height;
            label.text = coords.ToStringOnSeparateLines();
            label.color = c;
            label.fontSize = fontSize;
            label.rectTransform.localScale = Vector3.one * scale;
            r = label.rectTransform;
        }
    }
}
