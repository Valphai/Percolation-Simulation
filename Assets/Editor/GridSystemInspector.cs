using UnityEngine;
using UnityEditor;
using Grid;

[CustomEditor(typeof(GridSystem))]
public class GridSystemInspector : Editor	
{
    public int RunEnsembleTimes;
    private GridSystem grid;
    private void OnEnable()	
    {
        grid = (GridSystem)target;
    }
    public override void OnInspectorGUI()	
    {
        base.OnInspectorGUI();
        
        if (GUILayout.Button("Run"))
        {
            grid.Run();
        }
        if (GUILayout.Button("Run Ensemble"))
        {
            grid.RunEnsemble(grid.N, grid.L);
        }
        
    }
}