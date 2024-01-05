using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MouseCaster : MonoBehaviour
{
    [SerializeField] private float size;
    [SerializeField] private GameObject hitLocationIndicatorPrefab;
    private MouseIndicatorState _mouseIndicatorState;
    private Camera _camera;
    private readonly TrackingState _trackingState = new();
    [SerializeField] private GameObject planeIntersectionIndicatorPrefab;
    private GameObject _planeIntersectionIndicatorInstance;
    [SerializeField] private GameObject startPrefab;
    private GameObject _startInstance;
    [SerializeField] private MeshFilter[] meshesToCheckCollision;

    private void Start()
    {
        _camera = Camera.main;
        _mouseIndicatorState =
            new MouseIndicatorState(Instantiate(hitLocationIndicatorPrefab, Vector3.zero, Quaternion.identity));
        _planeIntersectionIndicatorInstance =
            Instantiate(planeIntersectionIndicatorPrefab, Vector3.zero, Quaternion.identity);
        _startInstance = Instantiate(startPrefab, Vector3.zero, Quaternion.identity);
    }

    // Update is called once per frame
    private void Update()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (_trackingState.CurrentlyTracking)
        {
            // If the mouse button is still down and we're tracking
            if (Input.GetMouseButton(0))
            {
                _mouseIndicatorState.Hide();
                Debug.Log("Moving everywhere");
                
                // I need that point in a plane parallel to the camera XY plane.
                // - Get the camera normal, and reverse it
                var planeNormal = -_camera.transform.forward;
                // - Get the point that we started at (1st mouse down)
                var point = _trackingState.InitialPosition;
                
                Debug.DrawRay(point, planeNormal * 2);
                
                if (Intersections.RayPlane(ray, point, planeNormal, out var hit))
                {
                    Debug.Log("Intersection at: " + hit.point);
                    _planeIntersectionIndicatorInstance.transform.position = hit.point;
                    _trackingState.UpdateIndices(hit.point);
                }
            }
        
            if (Input.GetMouseButtonUp(0))
            {
                _trackingState.StopTracking();
            }
        }
        else
        {
            CheckForMouseOverAndStart(ray);
        }
    }

    private void CheckForMouseOverAndStart(Ray ray)
    {
        // Will probably use something like this for VR
        if (Physics.Raycast(ray, out var mouseHit, 1000, LayerMask.GetMask("MovableCloth")))
        {
            var hitObject = mouseHit.transform.gameObject;
        
            // Debug.Log($"Mouse is over: {hitObject.name}");
        
            var worldSpacePosition = mouseHit.point;
        
            _mouseIndicatorState.Show();
            _mouseIndicatorState.UpdatePosition(worldSpacePosition, size);
            
            if (Input.GetMouseButtonDown(0))
            {
                _trackingState.StartTracking(worldSpacePosition, hitObject, size);
                _startInstance.transform.position = worldSpacePosition;
            }
        }
        else
        {
            // If we're not over the cloth, we for sure wont see anything
            _mouseIndicatorState.Hide();
        }
        
        // if (MyMath.Raycast(ray, meshesToCheckCollision.First(), out var houseHit))
        // {
        //     Debug.Log("Over!");
        // }
        // else
        // {
        //     Debug.Log("Not over");
        // }
    }

    class TrackingState
    {
        public bool CurrentlyTracking { get; private set; }
        public Vector3 InitialPosition { get; private set; }
        private GameObject _hitObject;
        private Mesh _meshToUpdate;
        private MeshCollider _meshCollider;
        private Dictionary<int, Vector3> _indicesAndOriginalPositions;

        public void StartTracking(Vector3 initialHitPosition, GameObject hitObject, float radius)
        {
            CurrentlyTracking = true;
            InitialPosition = initialHitPosition;
            _hitObject = hitObject;
            _meshToUpdate = hitObject.GetComponent<MeshFilter>().sharedMesh;
            _meshCollider = hitObject.GetComponent<MeshCollider>();

            bool LocalVertexInWorldHitRadius(Vector3 p) => 
                Vector3.Distance( hitObject.transform.TransformPoint(p), initialHitPosition) <= radius;
            
            _indicesAndOriginalPositions = _meshToUpdate.vertices
                .Select((v, i) => new { v, i })
                .Where(pair => LocalVertexInWorldHitRadius(pair.v))
                .ToDictionary(pair => pair.i, pair => pair.v);

            Debug.Log($"Finished starting tracking! Got {_indicesAndOriginalPositions.Count} indices.");
        }

        public void UpdateIndices(Vector3 worldMousePosition)
        {
            var localDelta = _hitObject.transform.InverseTransformVector(worldMousePosition - InitialPosition);
            var newPositions = _meshToUpdate.vertices;

            if (!_indicesAndOriginalPositions.Any()) return;
            
            foreach (var pair in _indicesAndOriginalPositions.Take(1))
            {
                newPositions[pair.Key] = pair.Value + localDelta;
            }

            UpdateMeshes(newPositions);
        }

        private void UpdateMeshes(Vector3[] newPositions)
        {
            _meshToUpdate.vertices = newPositions;
            _meshToUpdate.RecalculateBounds();
            _meshToUpdate.RecalculateNormals();
            
            // Need to assign the mesh every frame to get intersections happening correctly.
            // See: https://forum.unity.com/threads/how-to-update-a-mesh-collider.32467/
            _meshCollider.sharedMesh = _meshToUpdate;
        }

        public void StopTracking()
        {
            CurrentlyTracking = false;
        }
    }

    class MouseIndicatorState
    {
        private readonly GameObject _instance;
        private readonly LineRenderer _lineRenderer;

        public MouseIndicatorState(GameObject instance)
        {
            _instance = instance;
            _lineRenderer = instance.GetComponent<LineRenderer>();
        }

        public void UpdatePosition(Vector3 position, float size)
        {
            _instance.transform.position = position;
            _instance.transform.localScale = new Vector3(size, size, size);
        }

        public void Hide()
        {
            if (_lineRenderer.enabled)
            {
                _lineRenderer.enabled = false;
            }
        }

        public void Show()
        {
            _lineRenderer.enabled = true;
        }
    }
}