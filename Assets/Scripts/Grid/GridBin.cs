using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    [System.Serializable, ExecuteInEditMode]
    public class GridBin : MonoBehaviour
    {
        [SerializeField] public Coordinates Coordinates;
        public List<Disk> Disks { get; set; }
        [SerializeField] public GridBin[] neighbors;

        private void OnEnable()
        {
            Disks = new List<Disk>();
            CleanDisks();
        }
        public void AddDisk(Disk disk, UnionFind uf, int L)
        {
            float planeLength = Metrics.Diameter * L;

            Vector3 v1 = disk.Position;
            Vector3Int thisBinPos = Coordinates.IntVectorPositon();
            
            // check for intersecting disks in the same bin
            for (int i = 0; i < Disks.Count; i++)
            {
                Vector3 v2 = Disks[i].Position;

                // overlaps
                if (Vector3.Distance(v2, v1) < Metrics.Diameter)
                {
                    uf.Union(disk.DiskIndex, Disks[i].DiskIndex, L);
                }
            }

            Disks.Add(disk);

            // go through all neigbors
            foreach (GridBin neighBin in neighbors)
            {
                Vector3Int neighBinPos = neighBin.Coordinates.IntVectorPositon();
                bool binsFarApart = Vector3Int.Distance(thisBinPos, neighBinPos) > Mathf.Sqrt(2);

                // go through their disks
                for (int i = 0; i < neighBin.Disks.Count; i++)
                {
                    Disk neighbDisk = neighBin.Disks[i];
                    Vector3 v2 = neighbDisk.Position;

                    if (binsFarApart)
                    {
                        if ((neighBinPos - thisBinPos).x == -(L - 1))
                        {
                            v2 += Vector3.right * planeLength;
                        }
                        else if ((neighBinPos - thisBinPos).x == L - 1)
                        {
                            v2 -= Vector3.right * planeLength;
                        }
                        if ((neighBinPos - thisBinPos).z == -(L - 1))
                        {
                            v2 += Vector3.forward * planeLength;
                        }
                        else if ((neighBinPos - thisBinPos).z == L - 1)
                        {
                            v2 -= Vector3.forward * planeLength;
                        }
                    }
                    // overlaps
                    if (Vector3.Distance(v2, v1) < Metrics.Diameter)
                    {
                        uf.Union(disk.DiskIndex, neighbDisk.DiskIndex, L);
                    }
                }
            }
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
        public void CleanDisks()
        {
            Disks.Clear();
        }
    }
}

