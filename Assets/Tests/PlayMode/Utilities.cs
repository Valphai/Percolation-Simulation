using Grid;
using UnityEngine;
using TMPro;
using UnityEditor;

namespace PlayMode
{
    public class Utilities
    {
        public static GridSystem GridSetup(int numOfDisks = 600, int L = 40)
        {
            GameObject g = new GameObject();
            g.transform.position = Vector3.zero;

            GameObject p = new GameObject();
            p.transform.SetParent(g.transform, false);
            p.AddComponent(typeof(GridMesh));

            GameObject c = new GameObject();
            c.transform.SetParent(g.transform, false);
            c.AddComponent(typeof(Canvas));

            g.AddComponent(typeof(GridSystem));
            GridSystem grid = g.GetComponent<GridSystem>();
            grid.L = L;

            grid.binPrefab = AssetDatabase.LoadAssetAtPath<GridBin>("Assets/Prefabs/Grid/Bin.prefab");
            grid.diskPrefab = AssetDatabase.LoadAssetAtPath<Disk>("Assets/Prefabs/Grid/Disk.prefab");
            grid.labelPrefab = AssetDatabase.LoadAssetAtPath<TextMeshProUGUI>("Assets/Prefabs/Grid/Label.prefab");

            grid.SetupGrid(numOfDisks);
            
            return grid;
        }
    }
}