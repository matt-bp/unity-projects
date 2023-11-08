using Managers;
using UnityEngine;

namespace UI
{
    public class ReferencePositionCreationPopup : MonoBehaviour
    {
        public void Refresh()
        {
            var referencePositions = LoadedManagers.Rpm.GetReferencePositions();

            foreach (var (time, pos) in referencePositions)
            {
                Debug.Log($"Time {time}, Pos {pos}.");
            }
            
            // Show a transparent sphere prefab as to where the reference position is.
        }

        private float currentTime;
        public void AddTest()
        {
            Debug.Log("Adding test data from Reference Position Creation Popup");
            
            LoadedManagers.Rpm.AddReferencePosition(currentTime, currentTime);
            currentTime++;
        }
    }
}
