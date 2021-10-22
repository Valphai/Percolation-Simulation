using TMPro;
using UnityEngine;

namespace Grid
{
    public class GridSystem : MonoBehaviour
    {
        public int L;
        public GridBin[] bins { get; private set; }
        public UnionFind unionFind { get; private set; }
        [SerializeField] private int diskBoundLower, diskBoundHigher;
        [SerializeField] private float diskAddingTime;
        [SerializeField] private GridMesh gridMesh;
        [SerializeField] private GridBin binPrefab;
        [SerializeField] private Disk diskPrefab;
        [SerializeField] private TextMeshProUGUI labelPrefab;
        [SerializeField] private bool coroutineStart;
        [SerializeField] private Canvas gridCanvas;

        private void Awake() 
        {
    		gridCanvas = GetComponentInChildren<Canvas>();
    		gridMesh = GetComponentInChildren<GridMesh>();
    
    		bins = new GridBin[L * L];
    
    		for (int z = 0, i = 0; z < L; z++) 
            {
    			for (int x = 0; x < L; x++) 
                {
    				CreateBin(x, z, i++);
    			}
    		}
    	}
        private void Start() 
        {
    		gridMesh.Triangulate(bins);
            PopulateGrid();
    	}
    
        public void PopulateGrid()
        {
            // float[] distribution = Metrics.PoissonPointProcess(width, height, 
            //                                     diskBoundLower, diskBoundHigher);
            // for (int i = 0; i < distribution.Length; i++)
            // {
            //     int j = Random.Range(0, distribution.Length);
            //     int k = Random.Range(0, distribution.Length);
            //     AddDisk(distribution[j], distribution[k]);
            // }
            int n = Random.Range(diskBoundLower, diskBoundHigher);
            unionFind = new UnionFind(n);
           
            // for (int i = 0; i < n; i++)
            // {
            //     Disk disk = Instantiate<Disk>(diskPrefab);
            //     disk.DiskIndex = i;
            //     UnionFind.Disks[i] = disk;
            // }
            for (int i = 0; i < n; i++)
            {
                float x = Random.Range(0f, L * 2 - 1);
                float z = Random.Range(0f, L * 2 - 1);
                AddDisk(x, z, i, unionFind);
            }
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
    
            disk.Coordinates = Coordinates.FromVectorCoords(position);
    
            // find disk its on
            GetBin(disk.Coordinates).AddDisk(disk, unionFind, L);
    
            SetLabel(position, disk.Coordinates, Color.blue, out disk.UiRect,
                    Metrics.DiskFontSize, Metrics.DiskLabelHeight, Metrics.DiskRadius);
        }
        public GridBin GetBin(Coordinates coords) 
        {    
    		int index = coords.x + coords.z * L;
    		return bins[index];
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
