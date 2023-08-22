using UnityEngine;
using UnityEngine.Serialization;

namespace SimulationHelpers.Posing
{
    public class FramePoserUpdater : MonoBehaviour
    {
        public int vertexToUpdate;
        [FormerlySerializedAs("clothPoser")] public FramePoser framePoser;

        [SerializeField] private bool updating;
        
        public bool Updating {
            set => updating = value;
        }
    
        private void Update()
        {
            if (!updating) return;

            // Update 
            framePoser.UpdatePose(vertexToUpdate, transform.position);
        }
    }
}