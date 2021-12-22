using UnityEngine;

namespace Grid
{
    public class Disk : MonoBehaviour
    {
        public int DiskIndex { get; set; }
        public Coordinates Coordinates;
        public Vector3Int ToParentDisplacement;
        public RectTransform UiRect;
        public Vector3 Position
        {
            get { return transform.localPosition; }
            set { transform.localPosition = value; }
        }
        [SerializeField, HideInInspector]
        public MeshRenderer meshRenderer;
        private Color color;
        public Color Color
        {
            get
            {
                if (color == Color.yellow)
                {
                    // randomize color
                    color = new Color(
                        Random.value,
                        Random.value,
                        Random.value,
                        1);
                    meshRenderer.material.color = color;
                }
                return color;
            }
            set 
            { 
                color = value;
                var tempMaterial = new Material(meshRenderer.sharedMaterial);
                tempMaterial.color = color;
                meshRenderer.sharedMaterial = tempMaterial;
                // meshRenderer.material.color = color;
            }
        }
    
        // private void OnValidate()	
        // {
        //     if (!meshRenderer)
        //     {
        //         meshRenderer = GetComponent<MeshRenderer>();
        //     }
        //     // Position = Vector3.zero;

        //     transform.localScale = new Vector3(Metrics.DiskRadius * 2, Metrics.DiskHeight, Metrics.DiskRadius * 2);
        // }
    }
}
