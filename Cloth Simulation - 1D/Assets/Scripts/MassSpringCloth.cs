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
    // private Vector3 _position0;
    // private Vector3[] _forces;
    private float _force0y;
    // private Vector3[] _velocities;
    private float _velocity0y;

    private GameObject _vertex0Hint;
    private GameObject _vertex1Hint;
    
    #region Constants

    private readonly float gravity = 10;
    private readonly float k = 7;
    private readonly float restLength = 2.0f;
    private readonly int mass0 = 30;

    #endregion
    
    private void Start()
    {
        _mesh = GetComponent<MeshFilter>().mesh;
        _positions = _mesh.vertices;
        // _position0 = _positions[0];
        
        // _forces = FillEmpty(_positions.Length);
        // _velocities = FillEmpty(_positions.Length);

        _force0y = 0;
        _velocity0y = 0;

        // var temp = GetComponent<MeshFilter>();
        // var str = "";
        // foreach (var tri in temp.mesh.triangles)
        // {
        //     str += tri;
        // }
        // Debug.Log(str);

        // var vertices = temp.mesh.vertices;

        // vertices[0].y += 0.5f;
        //var pos = temp.mesh.vertices[0]; // 2 is ancor

        //pos.y += 0.5f;

        // temp.mesh.vertices = vertices;
        // temp.mesh.RecalculateBounds();
        //
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
        _vertex1Hint.transform.position = _mesh.vertices[2];
    }
    
    private void SimulationStep()
    {
        // compute forces
        ComputeForces();
        
        var a = _force0y / mass0;
    
        _velocity0y += a * Time.deltaTime;
    
        _positions[0].y += _velocity0y * Time.deltaTime;
    
        _mesh.vertices = _positions;
    }
    
    private void ComputeForces()
    {
        _force0y = 0;
        // _forces = FillEmpty(_positions.Length);
        //
        // _forces[0].y = mass0 * gravity;
        //
        // var position1 = _positions[0];
        // var position2 = _positions[2];
        //
        // var velocity1 = _velocities[0];
        // var velocity2 = _velocities[2];
        //
        // var springForce = ComputeSpringForce(position1.y, position2.y);
        // var dampingForce = ComputeDampingForce(position1.y, position2.y, velocity1.y, velocity2.y);
        //
        // _forces[0].y += springForce + dampingForce;
    }

    // private Vector3[] FillEmpty(int count)
    // {
    //     return Enumerable.Range(0, count)
    //         .Select(i => new Vector3())
    //         .ToArray();
    // }

    private float ComputeSpringForce(float pos1, float pos2)
    {
        var l = Math.Abs(pos2 - pos1);
        var d = (pos2 - pos1) / l;

        return k * (l - restLength) * d;
    }

    private float ComputeDampingForce(float y1, float y2, float v1, float v2)
    {
        var lDot = v2 - v1;
        var l = Math.Abs(y2 - y1);
        var d = (y2 - y1) / l;

        return k * lDot * d;
    }
}
