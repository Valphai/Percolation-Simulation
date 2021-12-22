using UnityEditor;
using UnityEngine;

namespace Grid
{
    public class PoolHelper : MonoBehaviour
    {
        [SerializeField]
        private Disk diskPrefab;
    
        private void OnValidate()	
        {
            diskPrefab = AssetDatabase.LoadAssetAtPath<Disk>("Assets/Prefabs/Grid/Disk.prefab");
        }
        public Disk CreateDisk() => Instantiate<Disk>(diskPrefab);
        public void TakeFromPool(Disk d) => d.gameObject.SetActive(true); // reset it maybe
        public void ReleaseFromPool(Disk d)
        {
            d.Color = Color.yellow;
            d.gameObject.SetActive(false);
        }
    }
}
