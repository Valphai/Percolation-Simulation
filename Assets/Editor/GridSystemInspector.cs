using UnityEngine;
using UnityEditor;
using Grid;
using UI;

[CustomEditor(typeof(GridSystem))]
public class GridSystemInspector : Editor	
{
    public int RunEnsembleTimes;
    private Controls controls;
    private GridSystem grid;
    public override void OnInspectorGUI()	
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
            if (check.changed)
            {
                controls.Visualize();
            }
        }
        if (GUILayout.Button("Visualize"))
        {
            controls.Visualize();
        }
        if (GUILayout.Button("Run Ensemble"))
        {
            // controls.RunEnsemble();
        }
        
    }
    private void OnEnable()	
    {
        controls = GameObject.FindWithTag("Controls").GetComponent<Controls>();
        grid = (GridSystem)target;
    }
}