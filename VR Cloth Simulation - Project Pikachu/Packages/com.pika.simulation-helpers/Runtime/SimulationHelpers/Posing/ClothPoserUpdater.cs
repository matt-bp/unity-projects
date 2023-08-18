using UnityEngine;

namespace SimulationHelpers.Posing
{
    public class ClothPoserUpdater : MonoBehaviour
    {
        public int vertexToUpdate;
        public ClothPoser clothPoser;
        public bool updating;
    
        private void Update()
        {
            if (!updating) return;

            // Update 
            clothPoser.UpdatePose(vertexToUpdate, transform.position);
        }
    }
}