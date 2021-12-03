using System.Collections.Generic;
using System.Reflection;
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
                
                if (System.Math.Abs(displacementP.x - displacementQ.x) == L - 1 ||
                    System.Math.Abs(displacementP.z - displacementQ.z) == L - 1)
                {
                    // if displacement vec differ by +- L => 
                    // cluster has a nontrivial winding number around one or
                    // both directions on the torus.
                    // if (System.Math.Abs(displacementP.x) == System.Math.Abs(displacementQ.x) ||
                    //     System.Math.Abs(displacementP.z) == System.Math.Abs(displacementQ.z))
                    // {
                        #region Debug
                            var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
                            var type = assembly.GetType("UnityEditor.LogEntries");
                            var method = type.GetMethod("Clear");
                            method.Invoke(new object(), null);
                            Debug.Log(displacementP);
                            Debug.Log(displacementQ);
                        #endregion
    
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
                            Disks[biggestRoot].Color = Color.cyan;
                        }
                    // }
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
            // var a = Vector3Int.zero;

            #region Path_Compression    
                var debugDistance = new List<Vector3>();

                int root = p;
                while (root != parent[root])
                {
                    // sum these displacements along the path traversed to find
                    // the total displacement to the root site.
                    
                    v1 += Disks[parent[root]].Coordinates.IntVectorPositon() - 
                        Disks[root].Coordinates.IntVectorPositon();

                    debugDistance.Add(Disks[parent[root]].Position);
                    debugDistance.Add(Disks[root].Position);
                    debugDistance.Add(v1);

                    root = parent[root];
                }
                    // if (2 * a.x > L - 1)
                    // {
                    //     a.x -= L;
                    // }
                    // else if (2 * System.Math.Abs(a.x) > L - 1)
                    // {
                    //     a.x += L;
                    // }
                    // if (2 * a.z > L - 1)
                    // {
                    //     a.z -= L;
                    // }
                    // else if (2 * System.Math.Abs(a.z) > L - 1)
                    // {
                    //     a.z += L;
                    // }
                    // v1 += a;
                    // debugDistance.Add(v1);

                // p.displacement = v1
                while (p != root) 
                {
                    // We also update all displacements along the path
                    // when we carry out the path compression
                    


                    int next = parent[p];
                    parent[p] = root;

                    // next.displacement = v1 -= p.displacement

                    // v1 += Disks[root].Coordinates.IntVectorPositon() - 
                    //         Disks[p].Coordinates.IntVectorPositon(); // bez sensu?

                    
                    p = next;
                }
                Distances.Add(debugDistance);

            #endregion
            return root;
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