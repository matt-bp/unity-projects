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
        // Compute forces
        var springForceY = -K * (_positions[0].y - _positions[2].y);
        var springForceX = -K * (_positions[0].x - _positions[2].x); 
        
        var dampingForceY = DampingCoef * _velocities[0].y;
        var dampingForceX = DampingCoef * _velocities[0].x;

        var force0Y = springForceY + Mass * Gravity - dampingForceY;
        var force0X = springForceX - dampingForceX;

        // Calculate new velocities and positions
        var a0Y = force0Y / Mass;
        _velocities[0].y += a0Y * Time.deltaTime;
        _positions[0].y += _velocities[0].y * Time.deltaTime;

        var a0X = force0X / Mass;
        _velocities[0].x += a0X * Time.deltaTime;
        _positions[0].x += _velocities[0].x * Time.deltaTime;
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