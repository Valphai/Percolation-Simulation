using UnityEngine;
using UnityEditor;
using Grid;

[CustomEditor(typeof(GridSystem))]
public class GridInspector : Editor	
{
    private UnionFind uf;
    private void OnEnable()	
    {
        uf = (GridSystem)target.unionFind;
    }
    public override void OnInspectorGUI()	
    {
        base.OnInspectorGUI();
    }
    private void OnSceneGUI()	
    {
        Handles.Label();
    }
}
