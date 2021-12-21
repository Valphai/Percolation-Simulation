using UnityEngine;
using UnityEditor;
using Grid;

[CustomEditor(typeof(Disk))]
public class DiskInspector : Editor	
{
    private Disk disk;
    private UnionFind uf;

    private void OnEnable()	
    {
        disk = (Disk)target;
        uf = GameObject.FindGameObjectWithTag("Grid")
            .GetComponent<GridSystem>().unionFind;
    }
    public override void OnInspectorGUI()	
    {
        base.OnInspectorGUI();
        GUILayout.Label("Coordinates: " + disk.Coordinates.ToString());
        GUILayout.Label("Displacement2Parent: " + disk.ToParentDisplacement.ToString());
    }
    private void OnSceneGUI()	
    {
        if (disk != null)
        {
            Handles.color = Color.magenta;
            var parentPos = uf.Disks[uf.parent[disk.DiskIndex]].Position;

            Handles.Label((disk.Position + parentPos) / 2f, 
                            disk.ToParentDisplacement.ToString());

            Handles.DrawLine(disk.Position, parentPos, 3f);
        }
    }
}