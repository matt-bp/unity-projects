using Managers;
using UnityEngine;

namespace UI
{
    public class MeshVertexUpdater : MonoBehaviour
    {
        public int vertexToUpdate = -1;
        public MeshFilter meshFilter;
        public int referencePositionIndex = -1;

        void Update()
        {
            Debug.Assert(vertexToUpdate >= 0 && vertexToUpdate < meshFilter.mesh.vertexCount);

            DoUpdate(transform.position);
        }

        private void DoUpdate(Vector3 position)
        {
            var localSpacePoint = meshFilter.gameObject.transform.InverseTransformPoint(position);
            
            // Updating local visualization
            {
                
                var mesh = meshFilter.mesh;
                // Local coordinate space
                var newVertices = mesh.vertices;

                // Convert world space into coord space.
                newVertices[vertexToUpdate] = localSpacePoint;

                mesh.vertices = newVertices;
                mesh.RecalculateBounds();
            }

            // Updating reference position
            {
                var newVertices = LoadedManagers.ReferencePositionManager.GetReferencePosition(referencePositionIndex);

                newVertices[vertexToUpdate] = localSpacePoint;
                
                LoadedManagers.ReferencePositionManager.UpdateReferencePosition(referencePositionIndex, newVertices);
            }
        }
    }
}
