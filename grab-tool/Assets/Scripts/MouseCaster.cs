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
                // _mouseIndicatorState.Show(); // because we might not be over a cloth
                // StopTrackingVertices();
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
                _trackingState.StartTracking(worldSpacePosition);
            }
            
            // If the mouse is down
            // Start tracking!
            // Record worldSpacePosition
            // Record hitObject
            // Record indices that were hit
            
            // if (Input.GetMouseButtonDown(0))
            // {
            //     if (!_trackingState.CurrentlyTracking)
            //         StartKeepingTrackOfVertices(hitObject.GetComponent<MeshFilter>().sharedMesh, hitObject.transform,
            //             worldSpacePosition);
            // }
        }
        else
        {
            // If we're not over the cloth, we for sure wont see anything
            _mouseIndicatorState.Hide();
        }
    }

    // private void StartKeepingTrackOfVertices(Mesh mesh, Transform hitObjectTransform, Vector3 worldSpacePosition)
    // {
    //     var indicesToUpdate = mesh.vertices
    //         .Select((v, i) => new { v = hitObjectTransform.TransformPoint(v), i })
    //         .Where(pair => Vector3.Distance(pair.v, worldSpacePosition) <= size)
    //         .Select(pair => pair.i).ToList();
    //
    //     Debug.Log($"Found {indicesToUpdate.Count} vertices");
    //     
    //     _trackingState = new TrackingState()
    //     {
    //         CurrentlyTracking = true,
    //         Mesh = mesh,
    //         StartingCursorWorldPosition = worldSpacePosition,
    //         IndicesToUpdate = indicesToUpdate
    //     };
    // }
    //
    // private void StopTrackingVertices()
    // {
    //     _trackingState = new TrackingState() { CurrentlyTracking = false };
    // }
    
    class TrackingState
    {
        public bool CurrentlyTracking { get; private set; }
        private Vector3 _initialPosition;
        
        public void StartTracking(Vector3 initialHitPosition)
        {
            CurrentlyTracking = true;
            _initialPosition = initialHitPosition;

            // Start tracking

            // Keep list of vertex indices that we hit
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