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
                return color;
            }
            set 
            { 
                color = value;
                var tempMaterial = new Material(meshRenderer.sharedMaterial);
                tempMaterial.color = color;
                meshRenderer.sharedMaterial = tempMaterial;
            }
        }
    }
}
