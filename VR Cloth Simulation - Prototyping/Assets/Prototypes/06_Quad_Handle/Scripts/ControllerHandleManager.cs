using Prototypes._06_Quad_Handle.Scripts;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Prototypes._06_Quad_Handle.Scripts
{
    public class ControllerHandleManager : MonoBehaviour
    {
        public InputActionReference controller;
        public InputActionReference buttonToPress;

        private readonly Collider[] activeColliders = new Collider[10];
        private int numActiveColliders = 0;
        [SerializeField]
        private XROrigin xrOrigin;
        [SerializeField]
        private LayerMask layerMask;

        private const float HitSphereRadius = 0.05f;
        
        // Update is called once per frame
        private void Update()
        {
            var buttonAction = buttonToPress.action;

            if (WasPressedThisFrame(buttonAction))
            {
                var controllerPosition = ReadAndAdjustControllerPosition();

                numActiveColliders = Physics.OverlapSphereNonAlloc(controllerPosition, HitSphereRadius, activeColliders, layerMask);
                
                Debug.Log("Hit " + numActiveColliders + " things at " + controllerPosition);

                for (var i = 0; i < numActiveColliders; i++)
                {
                    var activeCollider = activeColliders[i];
                    
                    Debug.Log(activeCollider.name);

                    if (activeCollider.gameObject.TryGetComponent(out FollowInputAction follower))
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
                    
                    Debug.Log(activeCollider.name);

                    if (activeCollider.gameObject.TryGetComponent(out FollowInputAction follower))
                    {
                        follower.EndFollowing();
                    }
                    else
                    {
                        Debug.Log("No component! Add a masking layer!");
                    }
                }

                numActiveColliders = 0;
            }
        }

        private Vector3 ReadAndAdjustControllerPosition()
        {
            var controllerPosition = controller.action.ReadValue<Vector3>();
            controllerPosition.y += xrOrigin.CameraYOffset;
            return controllerPosition;
        }

        private bool WasPressedThisFrame(InputAction action) => action.triggered && action.ReadValue<float>() > 0;

        private bool KeyUp(InputAction action) => action.ReadValue<float>() == 0;
    }
}
