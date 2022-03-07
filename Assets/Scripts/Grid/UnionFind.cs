// #define DEBUG_MODE
using UnityEngine;
#if DEBUG_MODE
using VisualDebugging;
#endif

namespace Grid
{
    public class UnionFind
    {
        /// <summary>Number of disks needed to create a cluster</summary>
        public int firstClusterN;
        public Disk[] Disks { get; private set; }
        public bool FirstClusterOccured { get; private set; }
        public int[] parent { get; private set; }
        /// <summary> size of each group </summary>
        private int[] size;

        /// <summary> number of groups </summary>
        private int count;
    
        public UnionFind(int n)
        {
#if DEBUG_MODE
            VisualDebug.Initialize();
#endif
            count = n;
            parent = new int[n];
            size = new int[n];
            Disks = new Disk[n];
        }

        public void Union(int p, int q, int L) 
        {
#if DEBUG_MODE
            VisualDebug.BeginFrame($"Union({p},{q})", true);
            VisualDebug.DrawPoint(Disks[p].Position, Metrics.DiskRadius);
            VisualDebug.DrawPoint(Disks[q].Position, Metrics.DiskRadius);
            // if (p == Disks.Length - 1) VisualDebug.Save();
#endif
            Vector3Int pToRootP;
            Vector3Int qToRootQ;

            // here we sum displacement vectors
            int rootP = Find(p, out pToRootP);
            int rootQ = Find(q, out qToRootQ);
    
            // in the same group
            if (rootP == rootQ) 
            {
                // if displacement vec differ by +- L => 
                // cluster has a nontrivial winding number around one or
                // both directions on the torus.
                if (pToRootP.x - qToRootQ.x >= L - 1 ||
                    qToRootQ.x - pToRootP.x >= L - 1 ||
                    pToRootP.z - qToRootQ.z >= L - 1 ||
                    qToRootQ.z - pToRootP.z >= L - 1)
                {
                    FirstClusterOccured = true;
                    firstClusterN = p;
#if DEBUG_MODE
                    int biggestRoot = Find(p);
                    VisualDebug.BeginFrame();
                    VisualDebug.SetColour(Colours.lightRed);
                    for (int i = 0; i < firstClusterN; i++)
                    {
                        if (Find(i) == biggestRoot)
                        {
                            VisualDebug.DrawPoint(Disks[i].Position, Metrics.DiskRadius);
                        }
                    }
                    VisualDebug.SetColour(Colours.lightBlue, Colours.veryDarkGrey);
                    VisualDebug.DrawPoint(Disks[q].Position, Metrics.DiskRadius);
                    VisualDebug.SetColour(Colours.lightGreen, Colours.veryDarkGrey);
                    VisualDebug.DrawPoint(Disks[p].Position, Metrics.DiskRadius);
                    VisualDebug.SetColour(Colours.white, Colours.veryDarkGrey);
                    VisualDebug.DrawPoint(Disks[biggestRoot].Position, Metrics.DiskRadius);

                    VisualDebug.Save();
#endif
                }

                return;
            }
    
            // make smaller root point to larger one
            if (size[rootP] < size[rootQ]) 
            {
                parent[rootP] = rootQ;
                size[rootQ] += size[rootP];

                Disks[rootP].ToParentDisplacement = PeriodicShiftVector(q, p,
                                                                qToRootQ, L);
#if DEBUG_MODE
                VisualDebug.BeginFrame("rootP->rootQ", true);
                VisualDebug.DontShowNextElementWhenFrameIsInBackground();
                VisualDebug.SetColour(Colours.darkRed, Colours.veryDarkGrey);
                VisualDebug.DrawPointWithLabel(Disks[p].Position, Metrics.DiskRadius, "p");
                VisualDebug.DrawPointWithLabel(Disks[q].Position, Metrics.DiskRadius, "q");
                VisualDebug.DrawPointWithLabel(Disks[rootP].Position, Metrics.DiskRadius, "rootP");
                VisualDebug.SetColour(Colours.lightBlue, Colours.veryDarkGrey);
                VisualDebug.DrawPointWithLabel(Disks[rootQ].Position, Metrics.DiskRadius, "rootQ");
                VisualDebug.SetColour(Colours.lightGreen, Colours.veryDarkGrey);
                VisualDebug.DrawLineSegmentWithLabel(Disks[rootP].Position, Disks[rootQ].Position, 
                                                Disks[rootP].ToParentDisplacement.ToString());
                VisualDebug.DrawLineSegmentWithLabel(Disks[q].Position, Disks[rootQ].Position, 
                                                Disks[q].ToParentDisplacement.ToString());
                VisualDebug.DrawLineSegmentWithLabel(Disks[p].Position, Disks[rootP].Position, 
                                                Disks[p].ToParentDisplacement.ToString());
#endif
            }
            else
            {
                parent[rootQ] = rootP;
                size[rootP] += size[rootQ];

                Disks[rootQ].ToParentDisplacement = PeriodicShiftVector(p, q,
                                                                pToRootP, L);
#if DEBUG_MODE                                                     
                VisualDebug.BeginFrame("rootQ->rootP", true);
                VisualDebug.DontShowNextElementWhenFrameIsInBackground();
                VisualDebug.SetColour(Colours.darkRed, Colours.veryDarkGrey);
                VisualDebug.DrawPointWithLabel(Disks[p].Position, Metrics.DiskRadius, "p");
                VisualDebug.DrawPointWithLabel(Disks[q].Position, Metrics.DiskRadius, "q");
                VisualDebug.DrawPointWithLabel(Disks[rootQ].Position, Metrics.DiskRadius, "rootQ");
                VisualDebug.SetColour(Colours.lightBlue, Colours.veryDarkGrey);
                VisualDebug.DrawPointWithLabel(Disks[rootP].Position, Metrics.DiskRadius, "rootP");
                VisualDebug.SetColour(Colours.lightGreen, Colours.veryDarkGrey);
                VisualDebug.DrawLineSegmentWithLabel(Disks[rootP].Position, Disks[rootQ].Position, 
                                                Disks[rootQ].ToParentDisplacement.ToString());
                VisualDebug.DrawLineSegmentWithLabel(Disks[q].Position, Disks[rootQ].Position, 
                                                Disks[q].ToParentDisplacement.ToString());
                VisualDebug.DrawLineSegmentWithLabel(Disks[p].Position, Disks[rootP].Position, 
                                                Disks[p].ToParentDisplacement.ToString());
#endif
            }
            count--;
            
            
        }
        private Vector3Int PeriodicShiftVector(int to, int from, 
            Vector3Int toRootTo, int L)
        {
            Vector3Int toPos = Disks[to].Coordinates.IntVectorPositon();
            Vector3Int fromPos = Disks[from].Coordinates.IntVectorPositon();
            Vector3Int newToX = Vector3Int.zero;
            Vector3Int newToZ = Vector3Int.zero;

            if ((toPos - fromPos).x == L - 1 || (toPos - fromPos).z == L - 1 ||
                (toPos - fromPos).x == -(L - 1) || (toPos - fromPos).z == -(L - 1))
            {
                if ((toPos - fromPos).x == -(L - 1))
                {
                    toPos += Vector3Int.right * L;
                }
                else if ((toPos - fromPos).x == L - 1)
                {
                    toPos -= Vector3Int.right * L;
                }
                if ((toPos - fromPos).z == -(L - 1))
                {
                    toPos += Vector3Int.forward * L;
                }
                else if ((toPos - fromPos).z == L - 1)
                {
                    toPos -= Vector3Int.forward * L;
                }
            }

            return -Disks[from].ToParentDisplacement + (toPos - fromPos) + toRootTo;
        }
        public int Find(int p, out Vector3Int v1) 
        {
            v1 = Vector3Int.zero;
            int startingP = p;

            int root = p;
            while (root != parent[root])
            {
                // When we compress and splint a path, we sum these
                // vectors to get the total displacement between each object
                // on the path and its new parent.

                v1 += Disks[root].ToParentDisplacement;
                root = parent[root];
            }

            Vector3Int v = v1;
            #region Path_Compression    
                while (p != root) 
                {
                    int next = parent[p];
                    Vector3Int old = Disks[p].ToParentDisplacement;
                    Disks[p].ToParentDisplacement = v;

                    parent[p] = root;
                    v -= old;
                    p = next;
                }

            #endregion
            return root;
        }
        public void TickDisk(Disk disk, int i)
        {
#if DEBUG_MODE
            VisualDebug.BeginFrame($"{i}", true);
            VisualDebug.SetColour(Colours.darkGrey, Colours.veryDarkGrey);
            VisualDebug.DrawPoint(disk.Position, Metrics.DiskRadius);
#endif
            disk.DiskIndex = i;
            Disks[i] = disk;

            parent[i] = i;
            size[i] = 1;
        }
    }
}