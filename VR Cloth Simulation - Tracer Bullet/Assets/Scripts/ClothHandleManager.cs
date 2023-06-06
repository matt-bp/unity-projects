using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClothHandleManager : MonoBehaviour
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
    
    private void Update()
    {
        var buttonAction = buttonToPress.action;

        if (buttonAction.WasPressedThisFrame() && buttonAction.IsPressed())
        {
            var controllerPosition = ReadAndAdjustControllerPosition();

            numActiveColliders =
                Physics.OverlapSphereNonAlloc(controllerPosition, HitSphereRadius, activeColliders, layerMask);
            
            Debug.Log("Hit " + numActiveColliders + " things at " + controllerPosition);

            for (var i = 0; i < numActiveColliders; i++)
            {
                var activeCollider = activeColliders[i];
                
                Debug.Log("Hit object was at " + activeCollider.gameObject.transform.position);
                    
                // Start having the game object follow this
                if (activeCollider.gameObject.TryGetComponent(out FollowInputAction follower))
                {
                    follower.StartFollowing(controller);
                }
                else
                {
                    Debug.Log("The handle won't follow the input action.");
                }

                // And have tings that should follow it start updating
                activeCollider.gameObject.GetComponent<UpdateClothVertex>().Updating = true;
            }
        }
        else if (buttonAction.WasReleasedThisFrame())
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

                activeCollider.gameObject.GetComponent<UpdateClothVertex>().Updating = false;
            }
                
            numActiveColliders = 0;
        }
    }
    
    private Vector3 ReadAndAdjustControllerPosition()
    {
        var controllerPosition = controller.action.ReadValue<Vector3>();
        controllerPosition.y += xrOrigin.CameraYOffset;
        var position = xrOrigin.gameObject.transform.position;
        controllerPosition.z += position.z;
        controllerPosition.x += position.x;
        return controllerPosition;
    }
}
