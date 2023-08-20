using System.Collections.Generic;
using SimulationHelpers.Geometry;
using UnityEngine;

namespace SimulationHelpers.Posing
{
    public class HandleCreator : MonoBehaviour
    {
        [SerializeField] private ClothPoser clothPoser;
        [SerializeField] private GameObject handlePrefab;
        private List<Vector3> initialPositions;
        

        private void Start()
        {
            var mesh = GetComponentInChildren<WorldSpaceMesh>();
            initialPositions = mesh.positions;
            Debug.Assert(mesh.positions.Count > 0);
            
            clothPoser.StartNewPose(initialPositions.Count);

            AddHandle(0);
        }
        
        private void AddHandle(int index)
        {
            // Transform local to world coords
            var worldPosition = transform.TransformPoint(initialPositions[index]);
            
            var handle = Instantiate(handlePrefab, worldPosition, Quaternion.identity);

            if (handle.TryGetComponent(out ClothPoserUpdater update))
            {
                update.clothPoser = clothPoser;
                update.vertexToUpdate = index;
            }
            else
            {
                Debug.Log("Handle didn't have the required component to update a mesh vertex");
                Destroy(handle);
            }
        }
    }
}