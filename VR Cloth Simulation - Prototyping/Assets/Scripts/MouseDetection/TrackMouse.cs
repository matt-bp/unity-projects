using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MouseDetection
{
    public class TrackMouse : MonoBehaviour
    {
        [SerializeField] private Camera Camera;
        
        private void Start()
        {
            Debug.Log("Sup");
        }

        private void Update()
        {
            var mousePos = Input.mousePosition;
            var ray = Camera.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out var hit))
            {
                Debug.Log("Found something!");  
                // TODO: See if we're on a grab point, if so, change its color
            }
            else
            {
                Debug.Log("Not over something");
            }
        }
    }
}