using System.Collections.Generic;
using SimulationHelpers.Geometry;
using UnityEngine;
using UnityEngine.Serialization;

namespace SimulationHelpers.Posing
{
    public class HandleCreator : MonoBehaviour
    {
        [FormerlySerializedAs("clothPoser")] [SerializeField] private FramePoser framePoser;
        [SerializeField] private GameObject handlePrefab;
        private List<Vector3> initialPositions;
        

        private void Start()
        {
            IWorldSpaceMesh mesh = GetComponentInChildren<WorldSpaceMesh>();
            initialPositions = mesh.GetPositions();
            Debug.Assert(initialPositions.Count > 0);
            
            framePoser.StartNewPose(initialPositions.Count);
            
            for(var i = 0; i < initialPositions.Count; i++)
                AddHandle(i);
        }
        
        private void AddHandle(int index)
        {
            // Transform local to world coords
            var worldPosition = transform.TransformPoint(initialPositions[index]);
            
            var handle = Instantiate(handlePrefab, worldPosition, Quaternion.identity);

            if (handle.TryGetComponent(out FramePoserUpdater update))
            {
                update.framePoser = framePoser;
                update.vertexToUpdate = index;
                
                framePoser.UpdatePose(index, worldPosition);
            }
            else
            {
                Debug.Log("Handle didn't have the required component to update a mesh vertex");
                Destroy(handle);
            }
        }
    }
}