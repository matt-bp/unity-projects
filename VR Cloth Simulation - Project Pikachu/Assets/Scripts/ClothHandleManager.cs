using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Facilitates the interaction between the VR controller, and the handle game objects.
///
/// I decided to use the VR controller game object, because the world coordinates didn't line up with what we were
/// getting back as the value from the controller position event. By using the game object, we can get the world
/// space coordinates without having to calculate them ourselves.
/// </summary>
public class ClothHandleManager : MonoBehaviour
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

        if (buttonAction.WasPressedThisFrame() && buttonAction.IsPressed())
        {
            var controllerPosition = controller.transform.position;
            Debug.Assert(controllerPosition != Vector3.zero); // Basically, want to make sure we're getting something here

            numActiveColliders =
                Physics.OverlapSphereNonAlloc(controllerPosition, HitSphereRadius, activeColliders, layerMask);
            
            Debug.Log("Hit " + numActiveColliders + " things at " + controllerPosition);

            for (var i = 0; i < numActiveColliders; i++)
            {
                var activeCollider = activeColliders[i];
                
                Debug.Log("Hit object was at " + activeCollider.gameObject.transform.position);
                    
                // Start having the game object follow this
                if (activeCollider.gameObject.TryGetComponent(out FollowTarget follower))
                {
                    follower.StartFollowing(controller);
                }
                else
                {
                    Debug.Log("The handle won't follow the input action.");
                }

                // And have things that should follow it start updating
                activeCollider.gameObject.GetComponent<UpdateClothVertex>().Updating = true;
            }
        }
        else if (buttonAction.WasReleasedThisFrame())
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
                    Debug.Log("Found an object, but it didn't have the " + nameof(FollowTarget) + " component.");
                }

                activeCollider.gameObject.GetComponent<UpdateClothVertex>().Updating = false;
            }
                
            numActiveColliders = 0;
        }
    }
}
