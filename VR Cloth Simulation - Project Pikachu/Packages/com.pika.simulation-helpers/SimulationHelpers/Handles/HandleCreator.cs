using System.Collections.Generic;
using SimulationHelpers.Geometry;
using UnityEngine;

namespace SimulationHelpers.Handles
{
    public class HandleCreator : MonoBehaviour
    {
        [SerializeField] private GameObject handlePrefab;
        private List<Vector3> initialPositions;
        private List<GameObject> handles = new();

        private void Start()
        {
            var mesh = GetComponentInChildren<WorldSpaceMesh>();
            initialPositions = mesh.positions;

            AddHandle(0);
        }
        
        private void AddHandle(int index)
        {
            // Transform local to world coords
            var worldPosition = transform.TransformPoint(initialPositions[index]);
            
            var handle = Instantiate(handlePrefab, worldPosition, Quaternion.identity);

            if (handle.TryGetComponent(out UpdateMeshVertex update))
            {
                update.vertexToUpdate = index;
                handles.Add(handle);
            }
            else
            {
                Debug.Log("Handle didn't have the required component to update a mesh vertex");
                Destroy(handle);
            }
        }
    }
}