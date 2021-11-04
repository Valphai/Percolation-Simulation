using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    public class GridMesh : MonoBehaviour
    {
        private List<Vector3> vertices;
        private List<int> triangles;
        private Mesh gridMesh;
        [SerializeField] private Material material;
    
        private void Awake()	
        {
            GetComponent<MeshFilter>().mesh = gridMesh = new Mesh();
            GetComponent<MeshRenderer>().material = material;
    		gridMesh.name = "Grid Mesh";
    
    		vertices = new List<Vector3>();
    		triangles = new List<int>();
        }
    
        public void Triangulate(GridBin[] bins)
        {
            vertices.Clear();
            triangles.Clear();
            for (int i = 0; i < bins.Length; i++)
            {
                Triangulate(bins[i]);
            }
    
            gridMesh.SetVertices(vertices);
            gridMesh.SetTriangles(triangles, 0);
    		gridMesh.RecalculateNormals();
        }
    
        private void Triangulate(GridBin bin)
        {
            Vector3 center = bin.transform.localPosition;
    
            Vector3 v1 = center + new Vector3(-Metrics.DiskRadius, 0f, Metrics.DiskRadius);
            Vector3 v2 = center + new Vector3(-Metrics.DiskRadius, 0f, -Metrics.DiskRadius);
            Vector3 v3 = center + new Vector3(Metrics.DiskRadius, 0f, Metrics.DiskRadius);
            Vector3 v4 = center + new Vector3(Metrics.DiskRadius, 0f, -Metrics.DiskRadius);
    
            AddQuad(v1, v2, v3, v4);
        }
        private void AddQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4) 
        {
    		int vertexIndex = vertices.Count;
    		vertices.Add(v1);
    		vertices.Add(v2);
    		vertices.Add(v3);
    		vertices.Add(v4);
    		triangles.Add(vertexIndex);
    		triangles.Add(vertexIndex + 2);
    		triangles.Add(vertexIndex + 1);
    		triangles.Add(vertexIndex + 1);
    		triangles.Add(vertexIndex + 2);
    		triangles.Add(vertexIndex + 3);
    	}
    }
}
