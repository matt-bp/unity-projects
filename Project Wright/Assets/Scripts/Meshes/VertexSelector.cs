using System;
using System.Collections.Generic;
using UnityEngine;

namespace Meshes
{
    [RequireComponent(typeof(MeshRenderer))]
    public class VertexSelector : MonoBehaviour
    {
        private MeshFilter _meshFilter;
        [SerializeField] private GameObject selectorPrefab;
        private Mesh _ourCopyMesh;
        
        private List<GameObject> _createdPrefabs = new();
        
        private void Start()
        {
            Init();
        }

        private void Update()
        {
            Debug.Log("VS update");
            
            CreateSelectors();
        }

        private void Init()
        {
            _meshFilter = GetComponent<MeshFilter>();

            CreateSelectors();
        }
        
        private void CreateSelectors()
        {
            ClearPrevious();
            
            foreach (var v in _meshFilter.sharedMesh.vertices)
            {
                var worldPos = gameObject.transform.TransformPoint(v);
                _createdPrefabs.Add(Instantiate(selectorPrefab, worldPos, Quaternion.identity));
            }
        }

        private void ClearPrevious()
        {
            foreach (var p in _createdPrefabs)
            {
                DestroyImmediate(p);
            }

            _createdPrefabs = new List<GameObject>();
        }
        
    }
}