using UnityEditor;
using UnityEngine;

namespace MouseDetection
{
    public class TrackMouse : MonoBehaviour
    {
        [SerializeField] private Camera Camera;
        [SerializeField] private GameObject targetPrefab;
        
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
                // TODO: See if we're on a grab point, if so, change its color
                var hitObject = hit.transform.gameObject;
                // if (hitObject is)
                var target = hitObject.GetComponent<GrabTarget>();
                if (target is not null)
                {
                    Debug.Log("Found target!");  
                }
                else
                {
                    Debug.Log("Not over something");
                }
            }
            else
            {
                Debug.Log("Not over something");
            }
        }
    }
}