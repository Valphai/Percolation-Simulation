using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public class GridBin : MonoBehaviour
    {
        public Coordinates Coordinates;
        public Vector3Int Position 
        {
            get
            { 
                return Vector3Int.FloorToInt(transform.localPosition); 
            }
        } // getter for mesh triangulation
        public List<Disk> disks { get; private set; }
        [SerializeField] private GridBin[] neighbors;
    
        private void Awake()	
        {
            disks = new List<Disk>();
            neighbors = new GridBin[8];
        }
        public void AddDisk(Disk disk, UnionFind uf, int L)
        {
            disks.Add(disk);

            // check for intersecting disks
            Vector3 v1 = disk.Position;

            // go through all neigbors
            foreach (GridBin bin in neighbors)
            {
                if (bin)
                {
                    for (int i = 0; i < bin.disks.Count; i++)
                    {
                        Vector3 v2 = bin.disks[i].Position;
                        Vector3 diskDistance = v2 - v1;
    
                        if (diskDistance.magnitude < 2f * Metrics.DiskRadius)
                        {
                            // overlaps
                            uf.Union(disk.DiskIndex, disk, 
                                    bin.disks[i].DiskIndex, bin.disks[i], L);
                        }
                    }
                }
            }
            // go through all their disks 
            // check if kolo pochodzace od pozycji dyskow overlap kola w disks List<Disk>

        }
    
        public GridBin GetNeighbor(Direction direction) 
        {
    		return neighbors[(int)direction];
    	}
        public void SetNeighbor(Direction direction, GridBin bin) 
        {
    		neighbors[(int)direction] = bin;
    		bin.neighbors[(int)direction.Opposite()] = this;
    	}
    }
}

