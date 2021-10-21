using UnityEngine;

namespace Grid
{
    public class Disk : MonoBehaviour
    {
        public int DiskIndex { get; set; }
        public Coordinates Coordinates;
        public Vector3 Position
        {
            get { return transform.localPosition; }
            set { transform.localPosition = value; }
        }
        private MeshRenderer meshRenderer;
        private Color color;
        public Color Color
        {
            get
            {
                if (color == Color.yellow)
                {
                    // randomize color
                    color = new Color(
                    Random.Range(0f, 1f),
                    Random.Range(0f, 1f),
                    Random.Range(0f, 1f),
                    1);
                    meshRenderer.material.color = color;
                }
                return color;
            }
            set 
            { 
                color = value;
                meshRenderer.material.color = color;
            }
        }
        public Vector3Int IntVectorPositon()
        {
            return new Vector3Int(Coordinates.x, 0, Coordinates.z);
        }
    
        private void Awake()	
        {
            meshRenderer = GetComponent<MeshRenderer>();
            Color = Color.yellow;
            GridSystem.Disks[DiskIndex] = this;

            transform.localScale = new Vector3(Metrics.DiskRadius * 2, .01f, Metrics.DiskRadius * 2);
        }
    }
}
