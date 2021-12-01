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
        for (int i = 0; i < uf.Distances.Count; i++)
        {
            var v2 = uf.Distances[i][0];
            var v3 = uf.Distances[i][1];
            var v1 = uf.Distances[i][2];

            Handles.DrawLine(v2, v3, 2f);
            Handles.Label((v3 + v2) / 2, v1.ToString());
        }
    }
}
