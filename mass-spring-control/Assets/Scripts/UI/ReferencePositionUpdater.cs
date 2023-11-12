using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(MeshFilter))]
    public class ReferencePositionUpdater : MonoBehaviour
    {
        [SerializeField] private GameObject handlePrefab;
        public int referencePositionIndex = -1;

        private List<GameObject> handles = new();
        
        public void CreateHandles()
        {
            var meshFilter = GetComponent<MeshFilter>();

            var pos = meshFilter.gameObject.transform.TransformPoint(meshFilter.mesh.vertices[0]);
            
            var handle = Instantiate(handlePrefab);
            handle.transform.position = pos;
            var updater = handle.GetComponent<MeshVertexUpdater>();
            updater.vertexToUpdate = 0;
            updater.referencePositionIndex = referencePositionIndex;
            updater.meshFilter = meshFilter;
            
            handles.Add(handle);
        }

        private void OnDestroy()
        {
            foreach (var handle in handles)
            {
                Destroy(handle);
            }

            handles = new List<GameObject>();
        }

        private void Start()
        {
            // previousPosition = this.gameObject.transform.position;
            // Spawn handles from here for each mesh vertex, and give it which index to go for 
        }

        private void Update()
        {
            // var currentPosition = this.gameObject.transform.position;
            // var delta = currentPosition - previousPosition;
            // Debug.Log($"Delta: {delta}");
            // previousPosition = currentPosition;
            //
            // var previousReference = LoadedManagers.ReferencePositionManager.GetReferencePosition(index);
            //
            // var newReference = previousReference.Select(v => v + delta).ToList();
            //
            // LoadedManagers.ReferencePositionManager.UpdateReferencePosition(index, newReference);


            // I need to keep track of the deltas in x, y, z for this reference position.
            // I then need to update the reference position with these deltas
            // Get the reference position for this index.
            // Apply deltas
            // Update reference position for this index with applied deltas

            // var newReferencePosition =
            //     new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
            // LoadedManagers.ReferencePositionManager.UpdateReferencePosition(index, newReferencePosition);
        }
    }
}
