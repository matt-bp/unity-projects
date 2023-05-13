using UnityEngine;
using UnityEngine.InputSystem;

namespace Prototypes._03_VR_In_Sphere.Scripts
{
    public class CheckControllerColliders : MonoBehaviour
    {
        private int Count = 0;
        public InputActionReference leftController;
        public InputActionReference buttonToPress;
        
        [SerializeField]
        private GameObject cameraFloorOffsetObject;
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

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
                var colliders = Physics.OverlapSphere(controllerPosition, 0.05f); // How can I visualize this? Maybe match he prefab I'm using for the hand?
                Debug.Log("Hit " + colliders.Length + " things at " + controllerPosition);

                foreach (var collider in colliders)
                {
                    Debug.Log(collider.name);
                }
            }
        }
    }
}
