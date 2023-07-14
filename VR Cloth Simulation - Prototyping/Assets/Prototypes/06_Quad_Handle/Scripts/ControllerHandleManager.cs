using UnityEngine;
using UnityEngine.InputSystem;

namespace Prototypes._06_Quad_Handle.Scripts
{
    /// <summary>
    /// Facilitates a VR game controller interacting with handles to move the cloth associated with that handle.
    ///
    /// The whole cloth will me moved, not an individual vertex.
    /// </summary>
    public class ControllerHandleManager : MonoBehaviour
    {
        #region Editor Fields
        
        public GameObject controller;
        public InputActionReference buttonToPress;

        [SerializeField]
        private LayerMask layerMask;
        
        #endregion
        
        private readonly Collider[] activeColliders = new Collider[10];
        private int numActiveColliders;
        private const float HitSphereRadius = 0.05f;
        
        private void Update()
        {
            var buttonAction = buttonToPress.action;

            if (WasPressedThisFrame(buttonAction))
            {
                var controllerPosition = controller.transform.position;

                numActiveColliders = Physics.OverlapSphereNonAlloc(controllerPosition, HitSphereRadius, activeColliders, layerMask);
                
                Debug.Log("Hit " + numActiveColliders + " things at " + controllerPosition);

                for (var i = 0; i < numActiveColliders; i++)
                {
                    var activeCollider = activeColliders[i];
                    
                    if (activeCollider.gameObject.TryGetComponent(out FollowTarget follower))
                    {
                        follower.StartFollowing(controller);
                    }
                }
            }
            else if (KeyUp(buttonAction))
            {
                for (var i = 0; i < numActiveColliders; i++)
                {
                    var activeCollider = activeColliders[i];
                    
                    if (activeCollider.gameObject.TryGetComponent(out FollowTarget follower))
                    {
                        follower.EndFollowing();
                    }
                    else
                    {
                        Debug.Log("Collider found, but no FollowTarget component attached to it.");
                    }
                }

                numActiveColliders = 0;
            }
        }

        private bool WasPressedThisFrame(InputAction action) => action.triggered && action.ReadValue<float>() > 0;

        private bool KeyUp(InputAction action) => action.ReadValue<float>() == 0;
    }
}
