using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public class MassSpringCloth : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private GameObject vertexNotification;
    public bool isEnabled = false;

    #endregion

    #region Private Fields

    private Mesh _mesh;
    private Vector3[] _positions;
    private Vector3[] _velocities;
    private Vector3[] _forces;
    private GameObject[] _hints;
    private Dictionary<int, bool> _anchors = new();

    #endregion

    #region Simulation Constants

    private const int K = 4;
    private const float DampingCoef = 1.0f;
    private const float Gravity = -10.0f;
    private const float Mass = 1.5f;
    private const float RestLength = 0.2f;

    #endregion

    private void Start()
    {
        _mesh = GetComponent<MeshFilter>().mesh;
        _positions = _mesh.vertices;
        _positions[0].x -= 0.2f;
        _positions[0].z += 0.5f;
        
        _positions[2].z -= 0.5f;

        _anchors[2] = true;
        _anchors[3] = true;

        _velocities = Enumerable.Range(0, _positions.Length).Select(_ => Vector3.zero).ToArray();
        _forces = Enumerable.Range(0, _positions.Length).Select(_ => Vector3.zero).ToArray();

        _hints = new GameObject[_mesh.vertexCount];

        AddHints();
        UpdateHints();
    }

    private void Update()
    {
        if (!isEnabled) return;
        
        SimulationStep();

        // Update mesh with new positions
        _mesh.vertices = _positions;
        _mesh.RecalculateBounds();

        UpdateHints();
    }

    #region Simulation

    private void SimulationStep()
    {
        ComputeForces();

        // Update velocities * positions
        for (var i = 0; i < _forces.Length; i++)
        {
            var acceleration = _forces[i] / Mass;
            _velocities[i] += acceleration * Time.deltaTime;
            _positions[i] += _velocities[i] * Time.deltaTime;
        }
    }

    private bool IsAnchor(int index)
    {
        return _anchors.TryGetValue(index, out var isAnchor);
    }

    private void ComputeForces()
    {
        _forces = _forces.Select(x => Vector3.zero).ToArray();

        // Set gravity contribution
        for (var i = 0; i < _forces.Length; i++)
        {
            if (IsAnchor(i)) continue;
            var massGravity = Gravity * Mass;
            _forces[i].y = massGravity;
        }

        for (var i = 0; i < _mesh.triangles.Length; i += 3)
        {
            var triangles = _mesh.triangles;
            ComputeForceForPair(triangles[i], triangles[i + 1]);
            ComputeForceForPair(triangles[i + 1], triangles[i + 2]);
            ComputeForceForPair(triangles[i + 2], triangles[i]);
        }
    }

    private void ComputeForceForPair(int first, int second)
    {
        var springForce = GetSpringForce(_positions[first], _positions[second]);
        
        var dampingForce = GetDampingForce(_velocities[first], _velocities[second]);

        if (!IsAnchor(first))
        {
            _forces[first] -= springForce - dampingForce;
        }

        if (!IsAnchor(second))
        {
            _forces[second] += springForce - dampingForce;
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
        return -DampingCoef * (velocity1 - velocity2);
    }

    #endregion

    #region Vertex Hints

    private void AddHints()
    {
        for (var i = 0; i < _mesh.vertexCount; i++)
        {
            _hints[i] = Instantiate(vertexNotification, _positions[i], Quaternion.identity);
        }
    }

    private void UpdateHints()
    {
        for (var i = 0; i < _hints.Length; i++)
        {
            _hints[i].transform.position = _positions[i];
        }
    }

    #endregion
}