using UnityEngine;
using System;
using System.Linq;
using Unity.VisualScripting;

public class MassSpringCloth : MonoBehaviour
{
    #region Editor Fields
    
    [SerializeField] private GameObject notification;
    [SerializeField] private GameObject redNotification;
    
    #endregion

    private Mesh _mesh;
    private Vector3[] _positions;
    private float _force0y;
    private float _velocity0y;

    private GameObject _vertex0Hint;
    private GameObject _vertex1Hint;
    
    #region Constants

    private readonly float gravity = -10;
    private readonly float k = 7;
    private readonly float dampingCoef = 1.0f;
    private readonly int mass0 = 1;

    #endregion
    
    private void Start()
    {
        _mesh = GetComponent<MeshFilter>().mesh;
        _positions = _mesh.vertices;

        _force0y = 0;
        _velocity0y = 0;

        _vertex0Hint = Instantiate(notification, _positions[0], Quaternion.identity);
        _vertex1Hint = Instantiate(redNotification, _positions[2], Quaternion.identity);
    }

    private void Update()
    {
        // Test moving mesh vertices
        // _positions[0].y += 0.5f * Time.deltaTime;

        SimulationStep();
        
        // Update mesh with new positions
        _mesh.vertices = _positions;
        _mesh.RecalculateBounds();
        
        // Update hints
        _vertex0Hint.transform.position = _positions[0];
        _vertex1Hint.transform.position = _positions[2];
    }

    private void SimulationStep()
    {
        var springForceY = -k * (_positions[0].y - _positions[2].y);

        var dampingForceY = dampingCoef * _velocity0y;
        
        _force0y = springForceY + mass0 * gravity - dampingForceY;

        var a = _force0y / mass0;

        _velocity0y += a * Time.deltaTime;

        _positions[0].y += _velocity0y * Time.deltaTime;

        _mesh.vertices = _positions;
    }
}
