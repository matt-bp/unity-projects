using UnityEngine;

namespace Prototypes._07_Quad_Vertex_Handle.Scripts
{
    public class UpdateMeshVertex : MonoBehaviour
    {
        [SerializeField]
        private MeshFilter meshFilter;
        [SerializeField] private int vertexToUpdate;
        public bool Updating { get; set; } = false;
        
        // Update is called once per frame
        void Update()
        {
            if (!Updating) return;
            
            Debug.Assert(vertexToUpdate >= 0 && vertexToUpdate < meshFilter.mesh.vertexCount);

            DoUpdate(transform.position);
        }

        private void DoUpdate(Vector3 position)
        {
            var mesh = meshFilter.mesh;
            // Local coordinate space
            var newVertices = mesh.vertices;
            
            // Convert world space into coord space.
            newVertices[vertexToUpdate] = meshFilter.gameObject.transform.InverseTransformPoint(position);

            mesh.vertices = newVertices;
            mesh.RecalculateBounds();
        }
    }
}
