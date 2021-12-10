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
        public int[] parent { get; private set; }
        private bool visualize;
    
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
            Vector3Int pToRootP;
            Vector3Int qToRootQ;

            // here we sum displacement vectors
            int rootP = Find(p, out pToRootP, L);
            int rootQ = Find(q, out qToRootQ, L);
    
            // in the same group
            if (rootP == rootQ) 
            {
                
                // if (System.Math.Abs(pToRootP.x) == System.Math.Abs(qToRootQ.x) ||
                //     System.Math.Abs(pToRootP.z) == System.Math.Abs(qToRootQ.z))
                // {
                // if displacement vec differ by +- L => 
                // cluster has a nontrivial winding number around one or
                // both directions on the torus.
                if (System.Math.Abs(pToRootP.x - qToRootQ.x) > L - 1 ||
                    System.Math.Abs(pToRootP.z - qToRootQ.z) > L - 1)
                {
                    #region Debug
                        var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
                        var type = assembly.GetType("UnityEditor.LogEntries");
                        var method = type.GetMethod("Clear");
                        method.Invoke(new object(), null);
                        Debug.Log(pToRootP);
                        Debug.Log(qToRootQ);
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
                }
                // }

                return;
            }
    
            // make smaller root point to larger one
            if (size[rootP] < size[rootQ]) 
            {
                parent[rootP] = rootQ;
                size[rootQ] += size[rootP];

                Disks[rootP].ToParentDisplacement = PeriodicShiftVector(q, p,
                                                    rootQ, rootP, qToRootQ, L);
            }
            else
            {
                parent[rootQ] = rootP;
                size[rootP] += size[rootQ];

                Disks[rootQ].ToParentDisplacement = PeriodicShiftVector(p, q,
                                                    rootP, rootQ, pToRootP, L);
            }
            count--;

        }

        // private Vector3Int PeriodicShiftVector(int p, int q, 
        //     int rootP, int rootQ, Vector3Int mToRootM, int L)
        // {
        private Vector3Int PeriodicShiftVector(int to, int from, 
            int rootTo, int rootFrom, Vector3Int toRootTo, int L)
        {
            Vector3Int toPos = Disks[to].Coordinates.IntVectorPositon();
            Vector3Int fromPos = Disks[from].Coordinates.IntVectorPositon();
            Vector3Int rTovec = Disks[rootTo].Coordinates.IntVectorPositon();
            Vector3Int rFromvec = Disks[rootFrom].Coordinates.IntVectorPositon();
            Vector3Int newRootX = Vector3Int.zero;
            Vector3Int newRootZ = Vector3Int.zero;

            if ((toPos - fromPos).x == L - 1 || (toPos - fromPos).z == L - 1 ||
                (toPos - fromPos).x == -(L - 1) || (toPos - fromPos).z == -(L - 1))
            {
                if ((toPos - fromPos).x == -(L - 1))
                {
                    newRootX = rTovec + Vector3Int.right * L;
                }
                else if ((toPos - fromPos).x == L - 1)
                {
                    newRootX = rTovec - Vector3Int.right * L;
                }
                if ((toPos - fromPos).z == -(L - 1))
                {
                    newRootZ = rTovec + Vector3Int.forward * L;
                }
                else if ((toPos - fromPos).z == L - 1)
                {
                    newRootZ = rTovec - Vector3Int.forward * L;
                }

                return newRootX + newRootZ - rFromvec;
            }

            return Disks[from].ToParentDisplacement + (toPos - fromPos) + toRootTo;
        }
        public int Find(int p, out Vector3Int v1, int L) 
        {
            v1 = Vector3Int.zero;
            var debugDistance = new List<Vector3>();

            int root = p;
            while (root != parent[root])
            {
                // sum these displacements along the path traversed to find
                // the total displacement to the root site.
                

                // debugDistance.Add(Disks[parent[root]].Position);
                // debugDistance.Add(Disks[root].Position);
                // debugDistance.Add(v1);

                v1 += Disks[root].ToParentDisplacement;

                root = parent[root];
            }

            #region Path_Compression    
                var v = v1;
                while (p != root) 
                {
                    // We also update all displacements along the path
                    // when we carry out the path compression

                    int next = parent[p];
                    parent[p] = root;

                    // next.displacement = v -= p.displacement
                    Disks[next].ToParentDisplacement = v -= Disks[p].ToParentDisplacement;
                    
                    p = next;
                }
                // p.displacement = v1
                Disks[p].ToParentDisplacement = v1;

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