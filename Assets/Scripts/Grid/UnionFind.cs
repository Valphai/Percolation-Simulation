using UnityEngine;

namespace Grid
{
    public class UnionFind
    {
        /// <summary>Number of disks needed to create a cluster</summary>
        public int firstClusterN;
        public Disk[] Disks { get; private set; }
        private bool visualize;
        private int[] parent;
    
        /// <summary> size of each group </summary>
        private int[] size;

        /// <summary> number of groups </summary>
        private int count;
    
        public bool FirstClusterOccured { get; private set; }
        public UnionFind(int n, bool visuals)
        {
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
            int rootP = Find(p, out displacementP);
            int rootQ = Find(q, out displacementQ);
    
            // in the same group
            if (rootP == rootQ) 
            {
                // if displacement vec differ by +- L => 
                displacementP = new Vector3Int(System.Math.Abs(displacementP.x),
                                                System.Math.Abs(displacementP.y),
                                                System.Math.Abs(displacementP.z));
                displacementQ = new Vector3Int(System.Math.Abs(displacementQ.x),
                                                System.Math.Abs(displacementQ.y),
                                                System.Math.Abs(displacementQ.z));
                if (displacementP.x + displacementQ.x >= L || 
                    displacementP.y + displacementQ.y >= L || 
                    displacementP.z + displacementQ.z >= L)
                {
                    // cluster has a nontrivial winding number around one or
                    // both directions on the torus.
                    FirstClusterOccured = true;
                    firstClusterN = p;

                    if (visualize)
                    {
                        for (int i = 0; i < Disks.Length; i++)
                        {
                            if (parent[i] == parent[p])
                            {
                                Disks[i].Color = Color.red;
                            }
                        }
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
        public int Find(int p, out Vector3Int v1) 
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
                while (p != parent[p])
                {
                    int next = parent[p];
                    parent[p] = parent[next];

                    v1 += Disks[next].Coordinates.IntVectorPositon() - 
                        Disks[p].Coordinates.IntVectorPositon();
                    
                    p = next;
                }
            #endregion
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