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

    private void Start()
    {
        _camera = Camera.main;
        _mouseIndicatorState =
            new MouseIndicatorState(Instantiate(hitLocationIndicatorPrefab, Vector3.zero, Quaternion.identity));
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
            
                // Update the vertices with how far we've come so far, from the beginning, not the last move.
            
                // Get where the cursor is now
            
                // I need how far it is from where it started
            
                // I need that point in a plane parallel to the camera XY plane.
                // - Get the camera normal, and reverse it
                // - Get the point that we started at (1st mouse down)
                // - Create a ray
                // - Intersect the ray with the plane
            
                // From intersection point, compute a difference to adjust points by.
                // - Add this difference to the points original position.
            
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
            }
        }
        else
        {
            // If we're not over the cloth, we for sure wont see anything
            _mouseIndicatorState.Hide();
        }
    }

    class TrackingState
    {
        public bool CurrentlyTracking { get; private set; }
        private Vector3 _initialPosition;
        private GameObject _hitObject;
        private Mesh _meshToUpdate;
        private Dictionary<int, Vector3> _indicesAndOriginalPositions;
        
        public void StartTracking(Vector3 initialHitPosition, GameObject hitObject, float radius)
        {
            CurrentlyTracking = true;
            _initialPosition = initialHitPosition;
            _hitObject = hitObject;
            _meshToUpdate = hitObject.GetComponent<MeshFilter>().sharedMesh;
            
            _indicesAndOriginalPositions = _meshToUpdate.vertices
                .Select((v, i) => new { v = hitObject.transform.TransformPoint(v), i })
                .Where(pair => Vector3.Distance(pair.v, initialHitPosition) <= radius)
                .ToDictionary(pair => pair.i, pair => pair.v);
            
            Debug.Log($"Finished starting tracking! Got {_indicesAndOriginalPositions.Count} indices.");
        }

        public void UpdateIndices(Vector3 offset)
        {
            var newPositions = _meshToUpdate.vertices;
            
            foreach (var pair in _indicesAndOriginalPositions)
            {
                // Start with direct offset, then add offset to original world space,
                // and convert back to local space for the mesh
                newPositions[pair.Key] += offset;
            }

            _meshToUpdate.vertices = newPositions;
            _meshToUpdate.RecalculateBounds();
            _meshToUpdate.RecalculateNormals();
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