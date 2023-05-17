using UnityEngine;
using UnityEngine.InputSystem;

namespace Prototypes._04_VR_Sphere_Follow.Scripts
{
    public class CheckControllerColliders : MonoBehaviour
    {
        private int Count = 0;
        public InputActionReference leftController;
        public InputActionReference buttonToPress;
        
        [SerializeField]
        private GameObject cameraFloorOffsetObject;

        private Collider[] activeColliders = new Collider[10];
        private int numActiveColliders = 0;
        
        // Update is called once per frame
        void Update()
        {
            var action = leftController.action;
            var buttonAction = buttonToPress.action;

            if (buttonAction.triggered && buttonAction.ReadValue<float>() > 0)
            {
                Count += 1;
                Debug.Log("primary press!" + Count);

                var controllerPosition = action.ReadValue<Vector3>();
                var offset = cameraFloorOffsetObject.transform.position.y;

                controllerPosition.y += offset;

                numActiveColliders = Physics.OverlapSphereNonAlloc(controllerPosition, 0.05f, activeColliders);
                //var colliders = Physics.OverlapSphere(controllerPosition, 0.05f); // How can I visualize this? Maybe match he prefab I'm using for the hand?
                Debug.Log("Hit " + numActiveColliders + " things at " + controllerPosition);

                for (var i = 0; i < numActiveColliders; i++)
                {
                    var collider = activeColliders[i];
                    
                    Debug.Log(collider.name);

                    if (collider.gameObject.TryGetComponent(out FollowTarget followTarget))
                    {
                        followTarget.StartFollowing(leftController);
                    }
                    else
                    {
                        Debug.Log("Noep!");
                    }
                }
            }
            else if (buttonAction.ReadValue<float>() == 0)
            {
                for (var i = 0; i < numActiveColliders; i++)
                {
                    var collider = activeColliders[i];
                    
                    Debug.Log(collider.name);

                    if (collider.gameObject.TryGetComponent(out FollowTarget followTarget))
                    {
                        followTarget.EndFollowing();
                    }
                    else
                    {
                        Debug.Log("No component! Add a maksing layer!");
                    }
                }

                numActiveColliders = 0;
            }
        }
    }
}
