using UnityEngine;

namespace Grid
{
    public class Disk : MonoBehaviour
    {
        public int DiskIndex { get; set; }
        public Coordinates Coordinates;
        public RectTransform UiRect;
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
    
        private void Awake()	
        {
            meshRenderer = GetComponent<MeshRenderer>();
            Color = Color.yellow;
            Position = Vector3.zero;

            transform.localScale = new Vector3(Metrics.DiskRadius * 2, .01f, Metrics.DiskRadius * 2);
        }
    }
}
