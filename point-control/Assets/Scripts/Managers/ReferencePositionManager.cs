using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class ReferencePositionManager : MonoBehaviour
    {
        private List<float> _referencePositions;
        private int _currentReference;
        
        public void SetReferencePositions(List<float> newReferences)
        {
            _referencePositions = newReferences;
        }

        public float GetCurrentReferencePosition()
        {
            return _referencePositions[_currentReference];
        }
    }
}