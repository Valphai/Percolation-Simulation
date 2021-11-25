using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public class GridBin : MonoBehaviour
    {
        public Coordinates Coordinates;
        public RectTransform UiRect;
        public List<Disk> Disks { get; private set; }
        private GridBin[] neighbors;
    
        private void Awake()
        {
            Disks = new List<Disk>();
            neighbors = new GridBin[8];
        }
        public void AddDisk(Disk disk, UnionFind uf, int L)
        {
            // check for intersecting disks
            for (int i = 0; i < Disks.Count; i++)
            {
                uf.Union(disk.DiskIndex, Disks[i].DiskIndex, L);
            }

            Disks.Add(disk);
            Vector3 v1 = disk.Position;

            // go through all neigbors
            foreach (GridBin bin in neighbors)
            {
                for (int i = 0; i < bin.Disks.Count; i++)
                {
                    if (uf.FirstClusterOccured) return;

                    Vector3 v2 = bin.Disks[i].Position;

                    Vector3 diskDistance = v2 - v1;

                    if (diskDistance.magnitude < 2 * Metrics.DiskRadius)
                    {
                        // overlaps
                        uf.Union(disk.DiskIndex, bin.Disks[i].DiskIndex, L);
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
        public void CleanDisks() => Disks.Clear();
    }
}

