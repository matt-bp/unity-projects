using UnityEngine;

namespace SimulationHelpers.Handles
{
    public class UpdateMeshVertex : MonoBehaviour
    {
        public int vertexToUpdate;
        public bool Updating { get; set; }
    
        private void Update()
        {
            if (!Updating) return;
            
            Debug.Log($"Going to move vertex {vertexToUpdate} to {transform.position}");
            
            // Update 
        }
    }
}