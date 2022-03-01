using UnityEngine;
using UnityEditor;
using Grid;

[CustomEditor(typeof(GridBin))]
public class GridBinEditor : Editor 
{
    private Coordinates coords;
    private GridBin bin;
    private void OnEnable() 
    {
        bin = (GridBin)target;
        coords = bin.Coordinates;
    }
    public override void OnInspectorGUI() 
    {
        base.OnInspectorGUI();
        
        GUILayout.Label("Coordinates: " + coords.ToString());
    }
}