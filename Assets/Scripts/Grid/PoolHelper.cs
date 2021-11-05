using TMPro;
using UnityEditor;
using UnityEngine;

namespace Grid
{
    public class PoolHelper : MonoBehaviour
    {
    
        private Disk diskPrefab;
    
        private void Awake()	
        {
            diskPrefab = AssetDatabase.LoadAssetAtPath<Disk>("Assets/Prefabs/Grid/Disk.prefab");
        }
        public Disk CreateDisk() => Instantiate<Disk>(diskPrefab);
        public void TakeFromPool(Disk d) => d.gameObject.SetActive(true); // reset it maybe
        public void ReleaseFromPool(Disk d) => d.gameObject.SetActive(false);
    }
}
