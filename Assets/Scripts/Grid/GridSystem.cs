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
        // [SerializeField] private int diskBoundLower, diskBoundHigher;
        // [SerializeField] private float diskAddingTime;
        [SerializeField] private GridMesh gridMesh;
        [SerializeField] public GridBin binPrefab;
        [SerializeField] public Disk diskPrefab;
        [SerializeField] public TextMeshProUGUI labelPrefab;
        // [SerializeField] private bool coroutineStart;
        [SerializeField] private Canvas gridCanvas;

        private void Awake()
        {
            gridCanvas = GetComponentInChildren<Canvas>();
            gridMesh = GetComponentInChildren<GridMesh>();
        }
        // private void Start() 
        // {
        //     SetupGrid(3000);
        // }
        public static GridSystem GridSetup(int numOfDisks = 600, int L = 40)
        {
            GameObject g = new GameObject();
            g.transform.position = Vector3.zero;

            GameObject p = new GameObject();
            p.transform.SetParent(g.transform, false);
            p.AddComponent(typeof(GridMesh));

            GameObject c = new GameObject();
            c.transform.SetParent(g.transform, false);
            c.AddComponent(typeof(Canvas));

            g.AddComponent(typeof(GridSystem));
            GridSystem grid = g.GetComponent<GridSystem>();
            grid.L = L;

            grid.binPrefab = AssetDatabase.LoadAssetAtPath<GridBin>("Assets/Prefabs/Grid/Bin.prefab");
            grid.diskPrefab = AssetDatabase.LoadAssetAtPath<Disk>("Assets/Prefabs/Grid/Disk.prefab");
            grid.labelPrefab = AssetDatabase.LoadAssetAtPath<TextMeshProUGUI>("Assets/Prefabs/Grid/Label.prefab");

            grid.SetupGrid(numOfDisks);
            
            return grid;
        }

        public void AddDisk(float x, float z, int i, UnionFind unionFind)
        {
            Vector3 position = new Vector3(
                x,
                Metrics.DiskHeight,
                z
            );
    
            Disk disk = Instantiate<Disk>(diskPrefab);
            unionFind.TickDisk(disk, i);
            
            disk.Position = position;
            disk.transform.SetParent(transform, true);

            disk.Coordinates = Coordinates.FromVectorCoords(position);
    
            // find bin its on
            GridBin bin = GetBin(disk.Coordinates);
            bin.AddDisk(disk, unionFind, L);
    
            SetLabel(position, disk.Coordinates, Color.blue, out disk.UiRect,
                    Metrics.DiskFontSize, Metrics.DiskLabelHeight, Metrics.DiskRadius);
        }
        public GridBin GetBin(Coordinates coords) 
        {    
    		int index = coords.x + coords.z * L;
    		return bins[index];
    	}
        public void SetupGrid(int numOfDisks)
        {
            CreateBins();
    		// gridMesh.Triangulate(bins);
            PopulateGrid(numOfDisks);
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
    
        private void PopulateGrid(int numOfDisks)
        {
            n = numOfDisks;
            unionFind = new UnionFind(n);
           
            for (int i = 0; i < n; i++)
            {
                if (unionFind.FirstClusterOccured) return;

                float x = UnityEngine.Random.Range(0f, L * 2 - 1);
                float z = UnityEngine.Random.Range(0f, L * 2 - 1);
                AddDisk(x, z, i, unionFind);
            }
        }
        private void CreateBin(int x, int z, int i)
        {
            int diameter = Metrics.DiskRadius * 2;
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
    
            SetLabel(position, bin.Coordinates, Color.white, out bin.UiRect,
                    Metrics.BinFontSize, Metrics.BinLabelHeight, diameter);
        }
    
        private void SetLabel(
            Vector3 position, Coordinates coords, Color c, out RectTransform r,
            float fontSize, float height, int scale)
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
