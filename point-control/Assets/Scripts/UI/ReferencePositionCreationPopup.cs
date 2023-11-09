using System.Collections.Generic;
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
            foreach (var ((time, pos), index) in referencePositions.WithIndex())
            {
                CreateAndAddReferencePositionVisualization(index, time, pos);
            }
        }

        private float currentTime = 0;

        public void AddTest()
        {
            Debug.Log("Adding test data from Reference Position Creation Popup");

            LoadedManagers.ReferencePositionManager.AddReferencePosition(currentTime, new Vector2(0, 0));
            currentTime+=5;
            
            Refresh(true);
        }

        private void CreateAndAddReferencePositionVisualization(int index, float time, Vector2 pos)
        {
            var refPosPrefab = Instantiate(referencePositionPrefab, new Vector3(pos.x, pos.y, 0),
                Quaternion.identity);
            refPosPrefab.GetComponent<ReferencePositionUpdater>().index = index;
            refPosPrefab.GetComponentInChildren<TMP_Text>().text = time.ToString();
            visualizedReferencePositions.Add(refPosPrefab);
        }
    }
}
