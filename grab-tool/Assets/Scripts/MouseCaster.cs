using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MouseCaster : MonoBehaviour
{
    [SerializeField] private float size;
    [SerializeField] private GameObject hitLocationIndicator;
    private GameObject hitLocationInstance;
    private Camera _camera;

    class TrackingState
    {
        public bool CurrentlyTracking { get; set; }
        public Mesh Mesh { get; set; }
        public Vector3 StartingCursorWorldPosition { get; set; }
        public List<int> IndicesToUpdate { get; set; }
    }

    private TrackingState _trackingState;

    private void Start()
    {
        _camera = Camera.main;
        hitLocationInstance = Instantiate(hitLocationIndicator, Vector3.zero, Quaternion.identity);
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

            UpdateCursorCircle(worldSpacePosition);
            
            if (Input.GetMouseButtonDown(0))
            {
                if (!_trackingState.CurrentlyTracking)
                    StartKeepingTrackOfVertices(hitObject.GetComponent<MeshFilter>().sharedMesh, hitObject.transform,
                        worldSpacePosition);
            }
        }

        if (Input.GetMouseButton(0))
        {
            Debug.Log("Moving everywhere");
            // Update the vertices with how far we've come so far, from the beginning, not the last move.
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            StopTrackingVertices();
        }
    }

    private void UpdateCursorCircle(Vector3 worldSpacePosition)
    {
        hitLocationInstance.transform.position = worldSpacePosition;
        hitLocationInstance.transform.localScale = new Vector3(size, size, size);
    }

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