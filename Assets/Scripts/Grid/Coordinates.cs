using UnityEngine;

namespace Grid
{
    public struct Coordinates
    {
        public int x { get; private set; }
        public int z { get; private set; }
        public Coordinates(int x, int z)
        {
            this.x = x;
            this.z = z;
        }
        public static Coordinates FromIntCoords(int x, int z) => new Coordinates(x, z);
        public static Coordinates FromVectorCoords(Vector3 position)
        {
            float x = position.x / (Metrics.DiskRadius * 2f);
            float z = position.z / (Metrics.DiskRadius * 2f);
            
            int iX = Mathf.RoundToInt(x);
            int iZ = Mathf.RoundToInt(z);

            if (iX <= 0) iX = 0;
            if (iZ <= 0) iZ = 0; 

    
            return new Coordinates(iX, iZ);
        }
        public static Vector3Int DisplacementDistance(Disk p, Disk parent, int L)
        {
            Vector3Int parentP = parent.Coordinates.IntVectorPositon();
            Vector3Int pP = p.Coordinates.IntVectorPositon();

            bool farApart = Vector3Int.Distance(parentP, pP) > Mathf.Sqrt(2);

            if (farApart)
            {
                if ((pP + Vector3Int.right).x > L - 1)
                {
                    parentP += Vector3Int.right * L;
                }
                else if ((pP - Vector3Int.right).x < 0)
                {
                    parentP -= Vector3Int.right * L;
                }
                if ((pP + Vector3Int.forward).z > L - 1)
                {
                    parentP += Vector3Int.forward * L;
                }
                else if ((pP - Vector3Int.forward).z < 0)
                {
                    parentP -= Vector3Int.forward * L;
                }
            }
            return parentP - pP;
        }
        public Vector3Int IntVectorPositon()
        {
            return new Vector3Int(x, 0, z);
        }
        public override string ToString() 
        {
    		return "(" + x.ToString() + ", " + z.ToString() + ")";
    	}
        public string ToStringOnSeparateLines()
        {
    		return x.ToString() + "\n" + z.ToString();
    	}
    }
}