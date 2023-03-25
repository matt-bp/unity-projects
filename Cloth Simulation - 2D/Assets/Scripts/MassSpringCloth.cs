using System;
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
    
    private void Start()
    {
        _mesh = GetComponent<MeshFilter>().mesh;
        _positions = _mesh.vertices;

        _hints = new GameObject[_mesh.vertexCount];

        for (var i = 0; i < _mesh.vertexCount; i++)
        {
            _hints[i] = Instantiate(vertexNotification, _positions[i], Quaternion.identity);    
        }
    }
}
