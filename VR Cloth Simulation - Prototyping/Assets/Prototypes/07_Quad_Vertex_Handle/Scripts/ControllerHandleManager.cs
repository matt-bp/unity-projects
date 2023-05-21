using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Prototypes._07_Quad_Vertex_Handle.Scripts
{
    public class ControllerHandleManager : MonoBehaviour
    {
        #region Editor Fields
        
        public InputActionReference controller;
        public InputActionReference buttonToPress;

        [SerializeField]
        private XROrigin xrOrigin;
        [SerializeField]
        private LayerMask layerMask;
        
        #endregion
        
        private readonly Collider[] activeColliders = new Collider[10];
        private int numActiveColliders = 0;
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
                    
                    if (activeCollider.gameObject.TryGetComponent(out FollowInputAction follower))
                    {
                        follower.StartFollowing(controller);
                    }

                    var update = activeCollider.gameObject.GetComponent<UpdateMeshVertex>();
                    update.Updating = true;
                }
            }
            else if (KeyUp(buttonAction))
            {
                for (var i = 0; i < numActiveColliders; i++)
                {
                    var activeCollider = activeColliders[i];
                    
                    if (activeCollider.gameObject.TryGetComponent(out FollowInputAction follower))
                    {
                        follower.EndFollowing();
                    }
                    else
                    {
                        Debug.Log("No component! Add a masking layer!");
                    }

                    activeCollider.gameObject.GetComponent<UpdateMeshVertex>().Updating = false;
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
