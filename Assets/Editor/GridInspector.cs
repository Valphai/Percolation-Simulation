using UnityEngine;
using UnityEditor;
using Grid;

[CustomEditor(typeof(GridSystem))]
public class GridInspector : Editor	
{
    private GridSystem grid;
    private UnionFind uf;
    private void OnEnable()	
    {
        grid = (GridSystem)target;
        uf = grid.unionFind;
    }
    public override void OnInspectorGUI()	
    {
        base.OnInspectorGUI();
    }
    private void OnSceneGUI()	
    {
        Handles.color = Color.blue;
        if (uf != null)
        {
            for (int i = 0; i < uf.Distances.Count; i++)
            {
                for (int j = 0; j < uf.Distances[i].Count - 2; j++)
                {
                    var v2 = uf.Distances[i][j];
                    var v3 = uf.Distances[i][j + 1];
                    var v1 = uf.Distances[i][j + 2];
        
                    Handles.DrawLine(v2, v3, 2f);
                    Handles.Label((v3 + v2) / 2, v1.ToString());
                }
            }
        }
    }
}
