using TMPro;
using UnityEngine;

namespace Grid
{
    public class GridSystem : MonoBehaviour
    {
        public int width;
        public int height;
        public GridBin[] bins { get; private set; }
        public UnionFind UnionFind  { get; private set; }
        [SerializeField] private GridMesh gridMesh;
        [SerializeField] private int diskBoundLower, diskBoundHigher;
        [SerializeField] private GridBin binPrefab;
        [SerializeField] private Disk diskPrefab;
        [SerializeField] private TextMeshProUGUI labelPrefab;
        [SerializeField] private Canvas gridCanvas;
    
        private void Awake() 
        {
    		gridCanvas = GetComponentInChildren<Canvas>();
    		gridMesh = GetComponentInChildren<GridMesh>();
    
    		bins = new GridBin[height * width];
    
    		for (int z = 0, i = 0; z < height; z++) 
            {
    			for (int x = 0; x < width; x++) 
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
            UnionFind = new UnionFind(n);

            for (int i = 0; i < n; i++)
            {
                float x = Random.Range(0f, width * 2 - 1);
                float z = Random.Range(0f, width * 2 - 1);
                AddDisk(x, z);
            }
        }
        public void AddDisk(float x, float z)
        {
            Vector3 position = new Vector3(
                x,
                Metrics.DiskHeight,
                z
            );
    
            Disk disk = Instantiate<Disk>(diskPrefab);
            disk.Position = position;
    
            disk.Coordinates = Coordinates.FromVectorCoords(position);
    
            // find disk its on
            GetBin(disk.Coordinates).AddDisk(disk);
    
            SetLabel(position, disk.Coordinates, Color.blue, 
                    Metrics.DiskFontSize, Metrics.DiskLabelHeight, Metrics.DiskRadius);
        }
        public GridBin GetBin(Coordinates coords) 
        {    
    		int index = coords.x + coords.z * width;
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
                bin.SetNeighbor(Direction.S, bins[i - width]);
                if (x > 0)
                {
                    bin.SetNeighbor(Direction.SW, bins[i - width - 1]);
                }
                if (x < width - 1)
                {
                    bin.SetNeighbor(Direction.SE, bins[i - width + 1]);
                }
            }
    
            SetLabel(position, bin.Coordinates, Color.white, 
                    Metrics.BinFontSize, Metrics.BinLabelHeight, diameter);
        }
    
        private void SetLabel(
            Vector3 position, Coordinates coords, Color c,
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
        }
    }
}
