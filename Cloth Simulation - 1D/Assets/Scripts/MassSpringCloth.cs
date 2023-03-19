using UnityEngine;
using System;
using System.Linq;

public class MassSpringCloth : MonoBehaviour
{
    #region Editor Fields
    
    [SerializeField] private GameObject notification;
    
    #endregion

    private Mesh _mesh;
    private Vector3[] _positions;
    private Vector3[] _forces;
    private Vector3[] _velocities;

    #region Constants

    private readonly float gravity = 10;
    private readonly float k = 7;
    private readonly float restLength = 2.0f;
    private readonly int[] masses = { 30, 0, 30, 0 };

    #endregion
    
    
    private void Start()
    {
        _mesh = GetComponent<MeshFilter>().mesh;
        _positions = _mesh.vertices;

        _forces = FillEmpty(_positions.Length);
        _velocities = FillEmpty(_positions.Length);

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
        Instantiate(notification, _positions[0], Quaternion.identity);
        Instantiate(notification, _positions[2], Quaternion.identity);
    }

    private void Update()
    {
        _positions[0].y += 0.5f * Time.deltaTime;

        _mesh.vertices = _positions;
        // SimulationStep();
    }

    private void SimulationStep()
    {
        // compute forces
        ComputeForces();

        // for each particle
        
        // update velocity

        // update position

        var a = _forces[0] / masses[0];

        _velocities[0] += a * Time.deltaTime;

        _positions[0] += _velocities[0] * Time.deltaTime;

        _mesh.vertices = _positions;
    }

    private void ComputeForces()
    {
        _forces = FillEmpty(_positions.Length);

        _forces[0].y = masses[0] * gravity;
        
        var position1 = _positions[0];
        var position2 = _positions[2];
        
        var velocity1 = _velocities[0];
        var velocity2 = _velocities[2];
        
        var springForce = ComputeSpringForce(position1.y, position2.y);
        var dampingForce = ComputeDampingForce(position1.y, position2.y, velocity1.y, velocity2.y);
        
        _forces[0].y += springForce + dampingForce;
    }

    private Vector3[] FillEmpty(int count)
    {
        return Enumerable.Range(0, count)
            .Select(i => new Vector3())
            .ToArray();
    }

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
