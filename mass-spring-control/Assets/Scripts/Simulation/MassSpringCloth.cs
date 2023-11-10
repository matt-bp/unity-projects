using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Simulation
{
    [RequireComponent(typeof(MeshFilter))]
    public class MassSpringCloth : MonoBehaviour
    {
        #region Simulation Constants

        private const int K = 4;
        private const float Kd = 1.0f;
        private const float Gravity = -10.0f;
        private const float Mass = 1.0f;
        private const float RestLength = 0.2f;

        #endregion

        private MeshFilter meshFilter;
        private Vector3[] initialPositions;

        [SerializeField] private Vector3[] positions;
        [SerializeField] private Vector3[] velocities;
        [SerializeField] private Vector3[] forces;
        [SerializeField] private List<int> constrainedIndices = new();
        [SerializeField] private List<SpringPair> springs = new();

        private void Start()
        {
            meshFilter = GetComponent<MeshFilter>();

            initialPositions = meshFilter.mesh.vertices.Select(v => meshFilter.gameObject.transform.TransformPoint(v))
                .ToArray();

            positions = initialPositions.Select(v => v).ToArray();
            velocities = Enumerable.Range(0, positions.Length).Select(_ => Vector3.zero).ToArray();
            forces = Enumerable.Range(0, positions.Length).Select(_ => Vector3.zero).ToArray();
        }

        public void Step(Vector3[] externalForces)
        {
            Assert.IsTrue(externalForces.Length == forces.Length);

            SimulationStep(externalForces);

            SetMeshVertices(positions);
        }

        private void SetMeshVertices(Vector3[] newPositions)
        {
            meshFilter.mesh.vertices =
                newPositions.Select(v => meshFilter.gameObject.transform.InverseTransformPoint(v)).ToArray();
            meshFilter.mesh.RecalculateBounds();
        }

        public Vector3[] Positions => positions;

        public void Reset()
        {
            positions = initialPositions.Select(v => v).ToArray();
            SetMeshVertices(initialPositions);
            velocities = Enumerable.Range(0, positions.Length).Select(_ => Vector3.zero).ToArray();
            forces = Enumerable.Range(0, positions.Length).Select(_ => Vector3.zero).ToArray();
        }

        private void SimulationStep(Vector3[] externalForces)
        {
            ComputeForces();

            for (var i = 0; i < externalForces.Length; i++)
            {
                forces[i] += externalForces[i];
            }

            // Update velocities * positions
            for (var i = 0; i < forces.Length; i++)
            {
                var acceleration = forces[i] / Mass;
                velocities[i] += acceleration * Time.deltaTime;
                positions[i] += velocities[i] * Time.deltaTime;
            }
        }

        private bool IsAnchor(int index)
        {
            return constrainedIndices.Contains(index);
        }

        private void ComputeForces()
        {
            forces = forces.Select(x => Vector3.zero).ToArray();

            // Set gravity contribution
            for (var i = 0; i < forces.Length; i++)
            {
                if (IsAnchor(i)) continue;

                forces[i].y = Gravity * Mass;
            }

            foreach (var pair in springs)
            {
                ComputeForceForPair(pair.firstIndex, pair.secondIndex);
            }
        }

        private void ComputeForceForPair(int first, int second)
        {
            var springForce = GetSpringForce(positions[first], positions[second]);

            var dampingForce = GetDampingForce(velocities[first], velocities[second]);

            if (!IsAnchor(first))
            {
                forces[first] -= springForce - dampingForce;
            }

            if (!IsAnchor(second))
            {
                forces[second] += springForce - dampingForce;
            }
        }

        private Vector3 GetSpringForce(Vector3 position1, Vector3 position2)
        {
            var distance = Vector3.Distance(position1, position2);
            var force = K * (distance - RestLength) * ((position1 - position2) / distance);
            return force;
        }

        private Vector3 GetDampingForce(Vector3 velocity1, Vector3 velocity2)
        {
            return -Kd * (velocity1 - velocity2);
        }
    }
}
