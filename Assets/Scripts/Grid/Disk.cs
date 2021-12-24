using UnityEngine;

namespace Grid
{
    public class Disk
    {
        public int DiskIndex { get; set; }
        public Coordinates Coordinates;
        public Vector3Int ToParentDisplacement;
        public Vector3 Position;
    }
}
