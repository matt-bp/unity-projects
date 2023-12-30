using System.Collections;
using UnityEngine;

namespace Projects.Procedural_Grid.Scripts
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class Grid : MonoBehaviour
    {
        public int xSize, ySize;
        private Vector3[] vertices;
        private Mesh mesh;
    
        private void Awake()
        {
            Generate();
        }

        private void Generate()
        {
            GetComponent<MeshFilter>().mesh = mesh = new Mesh();
            mesh.name = "Procedural Grid";
        
            vertices = new Vector3[(xSize + 1) * (ySize + 1)];
            var uv = new Vector2[vertices.Length];
            var tangents = new Vector4[vertices.Length];
            var tangent = new Vector4(1, 0, 0, -1);
            
            for (int i = 0, y = 0; y <= ySize; y++)
            {
                for (var x = 0; x <= xSize; x++, i++)
                {
                    vertices[i] = new Vector3(x, y);
                    uv[i] = new Vector2((float)x / xSize, (float)y / ySize);
                    tangents[i] = tangent;
                }
            }
        
            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.tangents = tangents;

            int[] triangles = new int[xSize * ySize * 6];

            for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++)
            {
                for (int x = 0; x < xSize; x++, ti += 6, vi++) {
                    triangles[ti] = vi;
                    triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                    triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
                    triangles[ti + 5] = vi + xSize + 2;

                    mesh.triangles = triangles;
                }  
            }
            
            mesh.triangles = triangles; // has to come after setting vertices
            mesh.RecalculateNormals();
        }

        private void OnDrawGizmos()
        {
            if (vertices is null)
            {
                return;
            }
        
            Gizmos.color = Color.black;
            for (var i = 0; i < vertices.Length; i++)
            {
                // Drawing in world space
                Gizmos.DrawSphere(vertices[i], 0.1f);
            }
        }
    }
}
