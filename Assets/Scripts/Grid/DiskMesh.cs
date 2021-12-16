using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    public class DiskMesh : MonoBehaviour
    {
        public Material Material;
        private List<Vector3> vertices;
        private List<int> triangles;
        private Mesh diskMesh;
        private static Vector3[] rotationArray = new Vector3[] {
            -Vector3.right,
            Vector3.forward,
            Vector3.right,
            -Vector3.forward,
            -Vector3.right
        };

        private void Awake()	
        {
            GetComponent<MeshFilter>().mesh = diskMesh = new Mesh();
            GetComponent<MeshRenderer>().material = Material;
    
    		vertices = new List<Vector3>();
    		triangles = new List<int>();
        }
        private void Start()	
        {
            Triangulate(Vector3.zero);
        }
        
        public DiskMesh(Vector3 point)
        {
            Triangulate(point);
        }
        public void Triangulate(Vector3 point)
        {
            vertices.Clear();
            triangles.Clear();

            TriangulateFans(point);

            diskMesh.SetVertices(vertices);
            diskMesh.SetTriangles(triangles, 0);
            diskMesh.RecalculateNormals();
        }

        private void TriangulateFans(Vector3 point)
        {
            float r = Metrics.DiskRadius;

            for (int i = 0; i < rotationArray.Length - 1; i++)
            {
                TriangulateFan(point, r, i);
            }
        }

        private void TriangulateFan(Vector3 point, float r, int i)
        {
            Vector3 v1 = point + rotationArray[i] * r;

            Vector3 corner = v1 + rotationArray[i + 1] * r;
            Vector3 v2 = Vector3.Lerp(v1, corner, .25f);
            Vector3 v3 = Vector3.Lerp(v1, corner, .7f);

            Vector3 v4 = point + rotationArray[i + 1] * r;
            Vector3 v5 = Vector3.Lerp(corner, v4, .25f);
            Vector3 v6 = Vector3.Lerp(corner, v4, .7f);

            Vector3 w2 = Circularize(point, r, v2);
            Vector3 w3 = Circularize(point, r, v3);
            Vector3 w5 = Circularize(point, r, v5);
            Vector3 w6 = Circularize(point, r, v6);

            int vertexIndex = vertices.Count;
            vertices.Add(point);
            vertices.Add(v1);
            vertices.Add(w2);
            vertices.Add(w3);
            vertices.Add(w5);
            vertices.Add(w6);
            vertices.Add(v4);
            triangles.Add(vertexIndex);
            triangles.Add(vertexIndex + 1);
            triangles.Add(vertexIndex + 2);
            triangles.Add(vertexIndex);
            triangles.Add(vertexIndex + 2);
            triangles.Add(vertexIndex + 3);
            triangles.Add(vertexIndex);
            triangles.Add(vertexIndex + 3);
            triangles.Add(vertexIndex + 4);
            triangles.Add(vertexIndex);
            triangles.Add(vertexIndex + 4);
            triangles.Add(vertexIndex + 5);
            triangles.Add(vertexIndex);
            triangles.Add(vertexIndex + 5);
            triangles.Add(vertexIndex + 6);
        }

        private static Vector3 Circularize(Vector3 point, float r, Vector3 edge)
        {
            return (edge - point).normalized * r;
        }
    }
}