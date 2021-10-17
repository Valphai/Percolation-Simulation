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
    
            return new Coordinates(iX, iZ);
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