using UnityEngine;

namespace Grid
{
    public class UnionFind
    {
        private int[] parent;
    
        /// <summary> size of each group </summary>
        private int[] size;
        // private Vector3Int[] displacementVectors;

        /// <summary> number of groups </summary>
        private int count;
    
        public UnionFind(int n)
        {
            count = n;
            parent = new int[n];
            size = new int[n];
            // displacementVectors = new Vector3Int[n];
    
            // parent nodes to themselves at the beggining
            for (int i = 0; i < n; i++) 
            {
                parent[i] = i;
                size[i] = 1;
            }
        }
        public void Union(int p, Disk dP, int q, Disk dQ, int L) 
        {
            Vector3Int displacementP;
            Vector3Int displacementQ;
            int rootP = Find(p, out displacementP); // these must return displacement vectors
            int rootQ = Find(q, out displacementQ);
    
            // in the same group
            if (rootP == rootQ) 
            {
                dP.Color = dQ.Color;
                // here we sum displacement vectors if they differ by +- L => 
                if (Mathf.Abs(displacementP.x - displacementQ.x) >= L ||
                    Mathf.Abs(displacementP.y - displacementQ.y) >= L ||
                    Mathf.Abs(displacementP.z - displacementQ.z) >= L)
                {
                    // cluster has a nontrivial winding number around one or
                    // both directions on the torus.
                    Debug.Log("Nontrivial");
                }

                return;
            }
    
            // make smaller root point to larger one
            if (size[rootP] < size[rootQ]) 
            {
                parent[rootP] = rootQ;
                size[rootQ] += size[rootP];

                dP.Color = dQ.Color;
                // displacementVectors[p] = dP.IntVectorPositon() - 
                //             GridSystem.Disks[rootQ].IntVectorPositon();
            }
            else 
            {
                parent[rootQ] = rootP;
                size[rootP] += size[rootQ];

                dQ.Color = dP.Color;
                // displacementVectors[q] = dQ.IntVectorPositon() - 
                //             GridSystem.Disks[rootP].IntVectorPositon();
            }
            count--;
        }
        public int Find(int p, out Vector3Int v1) 
        {
            v1 = Vector3Int.zero;
            // Validate(p);
    
            // int root = p;
            // while (root != parent[root])
            // {
            //     root = parent[root];
            // }
            
            // path compression
            // while (p != root) 
            // {
            //     int newP = parent[p];
            //     parent[p] = root;
            //     p = newP;
            // }

            // path splitting
            while (p != parent[p])
            {
                
                // if (GridSystem.Disks[p])
                // {
                //     displacementVectors[p] = GridSystem.Disks[p].IntVectorPositon();
                //     v1 += displacementVectors[p];
                // }

                int temp = parent[p];
                parent[p] = parent[parent[p]];
                p = temp;
            }
            return p;
        }
        // public bool Connected(int p, int q) 
        // {
        //     return Find(p) == Find(q);
        // }
        /// <summary>
        /// Validate that node p is in bounds
        /// </summary>
        private void Validate(int p) 
        {
            int n = parent.Length;
            if (p < 0 || p >= n) 
            {
                throw new System.Exception("index " + p + " is not between 0 and " + (n - 1));  
            }
        }
    }
    
}