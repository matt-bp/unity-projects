using System.Collections.Generic;
using System.Linq;
using Helpers;
using Unity.Mathematics;
using UnityEngine;

namespace MattMath._2D
{
    public class ImplicitMassSpring : MonoBehaviour
    {
        #region Simulation Constants

        [SerializeField] private double k = 10;
        [SerializeField] private double kd = 0.0;
        [SerializeField] private double l = 0.1;
        [SerializeField] private double2 gravity = math.double2(0.0, -10.0);
        /// <summary>
        /// This is the mass in kg for each particle. This will eventually be replaced by specifying the weight for the
        /// whole cloth, and then evenly distributing that across all particles.
        /// </summary>
        [SerializeField] private double m = 0.1;

        #endregion
        
        #region Simulation State

        private List<double2> forces;
        private List<double2> positions;
        private List<double2> velocities;
        private List<double> masses;
        private List<(int, int)> springs;

        public void SetPositionsAndSprings(List<double2> newPositions, List<(int, int)> newSprings)
        {
            forces = Grid<double2>.MakeVector(positions.Count, double2.zero);
            positions = Grid<double2>.MakeVector(positions.Count, double2.zero);
            velocities = Grid<double2>.MakeVector(velocities.Count, double2.zero);
            masses = Grid<double>.MakeVector(positions.Count, m);
            springs = newSprings.Select(x => (x.Item1, x.Item2)).ToList();
            
            Debug.Assert(IsSimulationStateValid());
        }
        
        private bool IsSimulationStateValid()
        {
            return positions.Count == forces.Count &&
                   positions.Count == velocities.Count &&
                   positions.Count == masses.Count &&
                   springs.All(pair =>
                       pair.Item1 >= 0 && pair.Item1 < positions.Count && pair.Item2 >= 0 &&
                       pair.Item2 < positions.Count);
        }
        
        #endregion

        public void StepSimulation(double dt)
        {
            SetForces();
        }
        
        private void SetForces()
        {
            // Gravitational force, also resets the force vector
            foreach (var index in Enumerable.Range(0, forces.Count))
            {
                forces[index] = gravity * masses[index];
            }
            
            // Spring force, should be added onto force vector
            
        }
    }
}