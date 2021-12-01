using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public class UnionFind
    {
        /// <summary>Number of disks needed to create a cluster</summary>
        public int firstClusterN;
        public Disk[] Disks { get; private set; }
        public bool FirstClusterOccured { get; private set; }
        public List<List<Vector3>> Distances { get; private set; }
        private bool visualize;
        private int[] parent;
    
        /// <summary> size of each group </summary>
        private int[] size;

        /// <summary> number of groups </summary>
        private int count;
    
        public UnionFind(int n, bool visuals)
        {
            Distances = new List<List<Vector3>>();

            visualize = visuals;
            count = n;
            parent = new int[n];
            size = new int[n];
            Disks = new Disk[n];
        }

        public void Union(int p, int q, int L) 
        {
            Vector3Int displacementP;
            Vector3Int displacementQ;

            // here we sum displacement vectors
            int rootP = Find(p, out displacementP, L);
            int rootQ = Find(q, out displacementQ, L);
    
            // in the same group
            if (rootP == rootQ) 
            {
                
                if (displacementP.x - displacementQ.x > L - 1 ||
                    displacementP.z - displacementQ.z > L - 1)
                {
                    // if displacement vec differ by +- L => 
                    // cluster has a nontrivial winding number around one or
                    // both directions on the torus.

                    FirstClusterOccured = true;
                    firstClusterN = p;

                    if (visualize)
                    {
                        int biggestRoot = Find(p);
                        for (int i = 0; i < firstClusterN; i++)
                        {
                            if (Find(i) == biggestRoot)
                            {
                                Disks[i].Color = Color.red;
                            }
                        }
                        Disks[p].Color = Color.black;
                        Disks[q].Color = Color.green;
                        Disks[biggestRoot].Color = Color.magenta;
                    }
                }

                return;
            }
    
            // make smaller root point to larger one
            if (size[rootP] < size[rootQ]) 
            {
                parent[rootP] = rootQ;
                size[rootQ] += size[rootP];
            }
            else 
            {
                parent[rootQ] = rootP;
                size[rootP] += size[rootQ];
            }
            count--;
        }
        public int Find(int p, out Vector3Int v1, int L) 
        {
            v1 = Vector3Int.zero;

            #region Path_Compression    
                // int root = p;
                // while (root != parent[root])
                // {
                //     root = parent[root];
                // }
                
                // int root = parent[p];
                // // path compression
                // while (p != root) 
                // {
                //     int newP = parent[p];
                //     parent[p] = root;
                //     p = newP;
                // }
            #endregion

            #region Path_Splitting
                var debugDistance = new List<Vector3>();
                while (p != parent[p])
                {
                    int next = parent[p];
                    parent[p] = parent[next];
                    
                    var distance = Disks[next].Coordinates.IntVectorPositon() - 
                            Disks[p].Coordinates.IntVectorPositon();

                    if (distance.magnitude <= Mathf.Sqrt(2))
                    {
                        v1 += distance;
                    }
                    else
                    {
                        if (distance.x > 1)
                        {
                            v1 += distance - Vector3Int.right * (L - 1);
                        }
                        else if (distance.x < -1)
                        {
                            v1 += distance + Vector3Int.right * (L - 1);
                        }
                        if (distance.z > 1)
                        {
                            v1 += distance - Vector3Int.forward * (L - 1);
                        }
                        else if (distance.z < -1)
                        {
                            v1 += distance + Vector3Int.forward * (L - 1);
                        }
                    }

                    debugDistance.Add(Disks[next].Position);
                    debugDistance.Add(Disks[p].Position);
                    debugDistance.Add(v1);
                    Distances.Add(debugDistance);

                    p = next;
                }
            #endregion
            return p;
        }
        public int Find(int p)
        {
            while (p != parent[p])
            {
                int next = parent[p];
                parent[p] = parent[next];
                
                p = next;
            }
            return p;
        }

        public void TickDisk(Disk disk, int i)
        {
            disk.DiskIndex = i;
            Disks[i] = disk;

            parent[i] = i;
            size[i] = 1;
        }
    }
}