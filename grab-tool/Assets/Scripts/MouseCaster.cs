using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MouseCaster : MonoBehaviour
{
    [SerializeField] private float size;
    [SerializeField] private GameObject hitLocationIndicator;
    // private GameObject _hitLocationInstance;
    private MouseIndicatorState _mouseIndicatorState;
    private Camera _camera;

    class TrackingState
    {
        public bool CurrentlyTracking { get; set; }
        public Mesh Mesh { get; set; }
        public Vector3 StartingCursorWorldPosition { get; set; }
        public List<int> IndicesToUpdate { get; set; }
    }

    class MouseIndicatorState
    {
        private GameObject _instance;
        private LineRenderer _lineRenderer;

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

    private TrackingState _trackingState;

    private void Start()
    {
        _camera = Camera.main;
        _mouseIndicatorState =
            new MouseIndicatorState(Instantiate(hitLocationIndicator, Vector3.zero, Quaternion.identity));
    }

    // Update is called once per frame
    private void Update()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var mouseHit, 1000, LayerMask.GetMask("MovableCloth")))
        {
            var hitObject = mouseHit.transform.gameObject;

            // Debug.Log($"Mouse is over: {hitObject.name}");

            var worldSpacePosition = mouseHit.point;

            _mouseIndicatorState.Show();
            _mouseIndicatorState.UpdatePosition(worldSpacePosition, size);
            
            // if (Input.GetMouseButtonDown(0))
            // {
            //     if (!_trackingState.CurrentlyTracking)
            //         StartKeepingTrackOfVertices(hitObject.GetComponent<MeshFilter>().sharedMesh, hitObject.transform,
            //             worldSpacePosition);
            // }
        }
        else
        {
            _mouseIndicatorState.Hide();
        }

        if (Input.GetMouseButton(0))
        {
            _mouseIndicatorState.Hide();
            Debug.Log("Moving everywhere");
            // Update the vertices with how far we've come so far, from the beginning, not the last move.
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            _mouseIndicatorState.Show();
            // StopTrackingVertices();
        }
    }

    // private void UpdateCursorCircle(Vector3 worldSpacePosition)
    // {
    //     _hitLocationInstance.transform.position = worldSpacePosition;
    //     _hitLocationInstance.transform.localScale = new Vector3(size, size, size);
    // }

    private void StartKeepingTrackOfVertices(Mesh mesh, Transform hitObjectTransform, Vector3 worldSpacePosition)
    {
        var indicesToUpdate = mesh.vertices
            .Select((v, i) => new { v = hitObjectTransform.TransformPoint(v), i })
            .Where(pair => Vector3.Distance(pair.v, worldSpacePosition) <= size)
            .Select(pair => pair.i).ToList();

        Debug.Log($"Found {indicesToUpdate.Count} vertices");
        
        _trackingState = new TrackingState()
        {
            CurrentlyTracking = true,
            Mesh = mesh,
            StartingCursorWorldPosition = worldSpacePosition,
            IndicesToUpdate = indicesToUpdate
        };
    }

    private void StopTrackingVertices()
    {
        _trackingState = new TrackingState() { CurrentlyTracking = false };
    }
}