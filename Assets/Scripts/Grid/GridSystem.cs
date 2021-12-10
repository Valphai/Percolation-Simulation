using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;


namespace Grid
{
    public class GridSystem : MonoBehaviour
    {
        public int L;
        /// <summary>number of disks</summary>
        public int n { get; private set; }
        public GridBin[] bins { get; private set; }
        public UnionFind unionFind { get; private set; }
        private bool visualize;
        private GridBin binPrefab;
        private TextMeshProUGUI labelPrefab;
        private GridMesh gridMesh;
        private Canvas gridCanvas;
        private ObjectPool<Disk> disksPool;
        [Range(.1f,15f)]
        public float interval;

        private void Awake()
        {
            gridCanvas = GetComponentInChildren<Canvas>();
            gridMesh = GetComponentInChildren<GridMesh>();

            binPrefab = AssetDatabase.LoadAssetAtPath<GridBin>("Assets/Prefabs/Grid/Bin.prefab");
            labelPrefab = AssetDatabase.LoadAssetAtPath<TextMeshProUGUI>("Assets/Prefabs/Grid/Label.prefab");
        }
        public void SetupGrid(int nMin, int nMax)
        {
    		n = UnityEngine.Random.Range(nMin, nMax);
            if (visualize)
                gridMesh.Triangulate(bins);

            PopulateGrid();
    	}

        /// <summary>Method for running tests</summary>
        public static GridSystem GridSetup(
            ObjectPool<Disk> disksPool, int nMin, int nMax, 
            int L = 40)
        {
            var g = AssignGrid(disksPool, L);

            g.CreateBins();
            g.SetupGrid(nMin, nMax);
            
            return g;
        }
        /// <summary>Microcanonical setup method</summary>
        public static GridSystem GridSetup(
            ref GridSystem g, ObjectPool<Disk> disksPool,
            int L = 16, bool visualize = false)
        {
            g.disksPool = disksPool;
            g.L = L;
            g.visualize = visualize;
            g.CreateBins();

            return g;
        }
        private static GridSystem AssignGrid(
            ObjectPool<Disk> disksPool, int L)
        {
            GameObject g = new GameObject();
            g.transform.position = Vector3.zero;

            g.AddComponent(typeof(GridSystem));
            GridSystem grid = g.GetComponent<GridSystem>();

            grid.disksPool = disksPool;
            grid.L = L;

            return grid;
        }
        public void ReleasePools()
        {
            for (int i = 0; i < unionFind.Disks.Length; i++)
            {
                if (unionFind.Disks[i])
                {
                    disksPool.Release(unionFind.Disks[i]);
                }
            }
            unionFind = null;
            Resources.UnloadUnusedAssets();
        }
        public void CleanBins()
        {
            foreach (GridBin bin in bins)
            {
                bin.CleanDisks();
            }
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
    
            Disk disk = disksPool.Get();
            uF.TickDisk(disk, i);
            
            disk.Position = position;
            disk.transform.SetParent(transform, true);

            disk.Coordinates = Coordinates.FromVectorCoords(position);
    
            // find the bin its on
            GridBin bin = GetBin(disk.Coordinates);
            bin.AddDisk(disk, uF, L);
    
            // if (visualize)
            // {
            //     SetLabel(position, disk.Coordinates, Color.blue, out disk.UiRect,
            //             Metrics.DiskFontSize, Metrics.DiskLabelHeight, Metrics.DiskRadius);
            // }
        }
        private GridBin GetBin(Coordinates coords) 
        {    
    		int index = coords.x + coords.z * L;
    		return bins[index];
    	}
        private void PopulateGrid()
        {
            unionFind = new UnionFind(n, visualize);
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
            float diameter = Metrics.DiskRadius * 2;
            Vector3 position = new Vector3(
                x * diameter,
                Metrics.BinHeight,
                z * diameter
            );
    
            GridBin bin = bins[i] = Instantiate<GridBin>(binPrefab);
            bin.transform.SetParent(transform, false);
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

            // if (visualize)
            // {
            //     SetLabel(position, bin.Coordinates, Color.white, out bin.UiRect,
            //             Metrics.BinFontSize, Metrics.BinLabelHeight, diameter);
            // }
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
