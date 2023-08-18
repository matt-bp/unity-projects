using UnityEngine;

namespace SimulationHelpers.Posing
{
    public class ClothPoserUpdater : MonoBehaviour
    {
        public int vertexToUpdate;
        public ClothPoser clothPoser;

        [SerializeField] private bool updating;
        
        public bool Updating {
            set => updating = value;
        }
    
        private void Update()
        {
            if (!updating) return;

            // Update 
            clothPoser.UpdatePose(vertexToUpdate, transform.position);
        }
    }
}