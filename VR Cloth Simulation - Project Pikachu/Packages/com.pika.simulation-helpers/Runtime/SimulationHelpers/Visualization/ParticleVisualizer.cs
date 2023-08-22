using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace SimulationHelpers.Visualization
{
    /// <summary>
    /// This component will create a game object at each of the simulation particle's positions.
    ///
    /// This won't visualize anything else, just points in space.
    /// </summary>
    [AddComponentMenu("Simulation Helpers/Visualization/Particle Visualizer")]
    public class ParticleVisualizer : Visualizer
    {
        [SerializeField] private GameObject particlePrefab;

        private List<GameObject> particles = new();

        public override void Clear()
        {
            Debug.Log($"{nameof(ParticleVisualizer)}: Resetting particle visualizer.");
            
            foreach (var particle in particles)
            {
                Destroy(particle);
            }

            particles = new List<GameObject>();
        }

        public override void Visualize(List<Vector3> positions, float elapsed, float dt)
        {
            if (particles.Count != positions.Count)
            {
                Debug.Log($"{nameof(ParticleVisualizer)}: Creating {positions.Count} new particles.");
                particles = new List<GameObject>();

                foreach (var t in positions)
                {
                    particles.Add(Instantiate(particlePrefab, t, Quaternion.identity));
                }
            }

            // Update positions of particles
            for (var i = 0; i < positions.Count; i++)
            {
                particles[i].transform.position = positions[i];
            }
        }
    }
}