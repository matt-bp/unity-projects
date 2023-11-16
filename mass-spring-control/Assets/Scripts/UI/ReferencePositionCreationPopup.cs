using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Managers;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ReferencePositionCreationPopup : MonoBehaviour
    {
        [SerializeField] private GameObject referencePositionPrefab;
        private List<GameObject> visualizedReferencePositions = new();
        
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
            foreach (var (value, index) in referencePositions.WithIndex())
            {
                VisualizeReferencePosition(index, value.Time, value.Position);
            }
        }

        private float currentTime = 0;

        public void AddNewReferencePosition()
        {
            StartCoroutine(CopyLastOrCreateAndUpdateTime());
        }

        private IEnumerator CopyLastOrCreateAndUpdateTime()
        {
            Debug.Log("Adding test data from Reference Position Creation Popup");

            var previousReferences = LoadedManagers.ReferencePositionManager.GetReferencePositions();

            if (!previousReferences.Any())
            {
                // Create
                throw new NotImplementedException();
            }
            else
            {
                // Copy last and update the time
                var mostRecentReferencePosition = previousReferences.Last();

                var newReferencePosition = new ReferencePositionManager.ReferencePosition
                {
                    Time = mostRecentReferencePosition.Time + 5,
                    Position = mostRecentReferencePosition.Position.ToList(),
                    EnabledVertices = mostRecentReferencePosition.EnabledVertices
                };

                LoadedManagers.ReferencePositionManager.AddReferencePosition(newReferencePosition);
            }
            
            // Wait one frame
            yield return null;

            Refresh(true);
        }

        private void VisualizeReferencePosition(int index, float time, List<Vector3> pos)
        {
            var refPosPrefab = Instantiate(referencePositionPrefab, Vector3.zero, Quaternion.identity);
            var refPositionUpdater = refPosPrefab.GetComponent<ReferencePositionUpdater>();
            refPositionUpdater.referencePositionIndex = index;
            refPosPrefab.GetComponentInChildren<TMP_Text>().text = time.ToString();
            
            // At some point, we'll need to change the triangles too
            var mesh = refPosPrefab.GetComponent<MeshFilter>().mesh;
            mesh.vertices = pos.Select(v => refPosPrefab.transform.InverseTransformPoint((v))).ToArray();
            mesh.RecalculateBounds();

            refPositionUpdater.CreateHandles();

            visualizedReferencePositions.Add(refPosPrefab);
        }
    }
}
