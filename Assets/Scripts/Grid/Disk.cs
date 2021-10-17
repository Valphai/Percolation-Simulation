using UnityEngine;

namespace Grid
{
    public class Disk : MonoBehaviour
    {
        public Color32 color;
        public Coordinates Coordinates;
        public Vector3 Position
        {
            get 
            { 
                return transform.localPosition;
            }
            set
            {
                transform.localPosition = value;
            }
        }
    
        private void Awake()	
        {
            transform.localScale = new Vector3(Metrics.DiskRadius * 2, .01f, Metrics.DiskRadius * 2);
        }
    }
}
