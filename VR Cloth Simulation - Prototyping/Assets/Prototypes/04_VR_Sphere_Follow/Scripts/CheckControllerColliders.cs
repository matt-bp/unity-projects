using UnityEngine;
using UnityEngine.InputSystem;

namespace Prototypes._04_VR_Sphere_Follow.Scripts
{
    public class CheckControllerColliders : MonoBehaviour
    {
        public InputActionReference buttonToPress;
        public GameObject leftController;

        private readonly Collider[] activeColliders = new Collider[10];
        private int numActiveColliders;
        private int count;
        
        // Update is called once per frame
        void Update()
        {
            //var action = leftController.action;
            var buttonAction = buttonToPress.action;

            if (buttonAction.triggered && buttonAction.ReadValue<float>() > 0)
            {
                count += 1;
                Debug.Log("primary press!" + count);

                var controllerPosition = leftController.transform.position;
                
                numActiveColliders = Physics.OverlapSphereNonAlloc(controllerPosition, 0.05f, activeColliders);
                //var colliders = Physics.OverlapSphere(controllerPosition, 0.05f); // How can I visualize this? Maybe match he prefab I'm using for the hand?
                Debug.Log("Hit " + numActiveColliders + " things at " + controllerPosition);

                for (var i = 0; i < numActiveColliders; i++)
                {
                    var currentCollider = activeColliders[i];
                    
                    if (currentCollider.gameObject.TryGetComponent(out FollowTarget followTarget))
                    {
                        followTarget.StartFollowing(leftController);
                    }
                    else
                    {
                        Debug.Log("No follow target component on " + currentCollider.name);
                    }
                }
            }
            else if (buttonAction.ReadValue<float>() == 0)
            {
                for (var i = 0; i < numActiveColliders; i++)
                {
                    var currentCollider = activeColliders[i];
                    
                    Debug.Log(currentCollider.name);

                    if (currentCollider.gameObject.TryGetComponent(out FollowTarget followTarget))
                    {
                        followTarget.EndFollowing();
                    }
                    else
                    {
                        Debug.Log("No component! Add a masking layer!");
                    }
                }

                numActiveColliders = 0;
            }
        }
    }
}
