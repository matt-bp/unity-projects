using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Managers;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace UI
{
    public class ReferencePositionCreationPopup : MonoBehaviour
    {
        [SerializeField] private GameObject referencePositionPrefab;
        private List<GameObject> visualizedReferencePositions = new();

        [SerializeField] private MeshFilter defaultMeshFilter;
        
        public void Refresh(bool createVisualization)
        {
            // Clear out previous visualization
            foreach (var previousObject in visualizedReferencePositions)
            {
                Destroy(previousObject);
            }

            visualizedReferencePositions = new List<GameObject>();

            if (!createVisualization) return;

            // Show current visualization
            var referencePositions = LoadedManagers.ReferencePositionManager.GetReferencePositions();
            foreach (var ((time, pos), index) in referencePositions.WithIndex())
            {
                CreateAndAddReferencePositionVisualization(index, time, pos);
            }
        }

        private float currentTime = 0;

        public void AddTest()
        {
            StartCoroutine(AddDefaultPositionAtCurrentTime());
        }

        private IEnumerator AddDefaultPositionAtCurrentTime()
        {
            Debug.Log("Adding test data from Reference Position Creation Popup");

            LoadedManagers.ReferencePositionManager.AddReferencePosition(currentTime, defaultMeshFilter.mesh.vertices.Select(v => v).ToList());
            currentTime += 5;

            // Wait one frame
            yield return null;

            Refresh(true);
        }

        private void CreateAndAddReferencePositionVisualization(int index, float time, List<Vector3> pos)
        {
            var refPosPrefab = Instantiate(referencePositionPrefab, Vector3.zero, Quaternion.identity);
            refPosPrefab.GetComponent<ReferencePositionUpdater>().index = index;
            refPosPrefab.GetComponentInChildren<TMP_Text>().text = time.ToString();
            
            // At some point, we'll need to change the triangles too
            var mesh = refPosPrefab.GetComponent<MeshFilter>().mesh;
            mesh.vertices = pos.Select(v => refPosPrefab.transform.InverseTransformPoint((v))).ToArray();
            mesh.RecalculateBounds();

            visualizedReferencePositions.Add(refPosPrefab);
        }
    }
}
