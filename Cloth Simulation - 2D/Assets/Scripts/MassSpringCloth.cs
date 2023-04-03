using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public class MassSpringCloth : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private GameObject vertexNotification;

    #endregion

    #region Private Fields

    private Mesh _mesh;
    private Vector3[] _positions;
    private Vector2[] _velocities;
    private Vector2[] _forces;
    private GameObject[] _hints;

    #endregion

    #region Simulation Constants

    private const int K = 7;
    private const float DampingCoef = 1.0f;
    private const float Gravity = -10.0f;
    private const int Mass = 1;

    #endregion

    private void Start()
    {
        _mesh = GetComponent<MeshFilter>().mesh;
        _positions = _mesh.vertices;
        _positions[0].x -= 2.0f;

        _velocities = Enumerable.Range(0, _positions.Length).Select(_ => Vector2.zero).ToArray();
        _forces = Enumerable.Range(0, _positions.Length).Select(_ => Vector2.zero).ToArray();

        _hints = new GameObject[_mesh.vertexCount];

        AddHints();
    }

    private void Update()
    {
        SimulationStep();

        // Update mesh with new positions
        _mesh.vertices = _positions;
        _mesh.RecalculateBounds();

        UpdateHints();
    }

    #region Simulation

    private void SimulationStep()
    {
        _forces = _forces.Select(x => Vector2.zero).ToArray();

        // Compute forces
        var springForceY = -K * (_positions[0].y - _positions[2].y);
        var springForceX = -K * (_positions[0].x - _positions[2].x);

        var dampingForceY = DampingCoef * _velocities[0].y;
        var dampingForceX = DampingCoef * _velocities[0].x;

        _forces[0] = new Vector2(springForceX - dampingForceX, springForceY + Mass * Gravity - dampingForceY);


        var acceleration = new Vector2(_forces[0].x / Mass, _forces[0].y / Mass);
        _velocities[0] += acceleration * Time.deltaTime;
        _positions[0] += new Vector3(
            _velocities[0].x * Time.deltaTime,
            _velocities[0].y * Time.deltaTime,
            0);
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