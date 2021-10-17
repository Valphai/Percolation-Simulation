public class UnionFind
{
    private int[] parent;

    /// <summary> size of each group </summary>
    private int[] size;

    /// <summary> number of groups </summary>
    private int count;

    public UnionFind(int n)
    {
        count = n;
        parent = new int[n];
        size = new int[n];

        // parent nodes to themselves at the beggining
        for (int i = 0; i < n; i++) 
        {
            parent[i] = i;
            size[i] = 1;
        }
    }
    public void Union(int p, int q) 
    {
        int rootP = Find(p);
        int rootQ = Find(q);

        if (rootP == rootQ) return;

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
    public int Find(int p) 
    {
        Validate(p);

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
            int temp = parent[p];
            parent[p] = parent[parent[p]];
            p = temp;
        }
        return p;
    }
    public bool Connected(int p, int q) 
    {
        return Find(p) == Find(q);
    }
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
