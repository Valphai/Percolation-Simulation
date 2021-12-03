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
    }
    private void OnSceneGUI()	
    {
        if (disk != null && uf.FirstClusterOccured)
        {
            var parentPos = uf.Disks[uf.parent[disk.DiskIndex]].Position;
    
            Handles.color = Color.cyan;

            Handles.DrawLine(disk.Position, parentPos, 3f);

            Handles.Label((disk.Position + parentPos) / 2f, 
                            disk.ToParentDisplacement.ToString());
        }
    }
}