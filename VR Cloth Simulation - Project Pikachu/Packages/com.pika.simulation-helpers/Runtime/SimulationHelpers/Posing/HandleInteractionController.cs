using UnityEngine;
using UnityEngine.InputSystem;

namespace SimulationHelpers.Posing
{
    /// <summary>
    /// Facilitates the interaction between the VR controller, and the handle game objects.
    ///
    /// I decided to use the VR controller game object, because the world coordinates didn't line up with what we were
    /// getting back as the value from the controller position event. By using the game object, we can get the world
    /// space coordinates without having to calculate them ourselves.
    /// </summary>
    [AddComponentMenu("Simulation Helpers/Posing/Handle Interaction Controller")]
    public class HandleInteractionController : MonoBehaviour
    {
        #region Editor Fields

        public GameObject vrController;
        public InputActionReference buttonToPress;

        [SerializeField] private LayerMask layerMask;

        #endregion

        private readonly Collider[] activeColliders = new Collider[10];
        private int numActiveColliders;
        private const float HitSphereRadius = 0.05f;

        private void Update()
        {
            var buttonAction = buttonToPress.action;

            if (buttonAction.WasPressedThisFrame() && buttonAction.IsPressed())
            {
                FindAndStartFollowers();
            }
            else if (buttonAction.WasReleasedThisFrame())
            {
                StopCurrentFollowers();
            }
        }

        private void FindAndStartFollowers()
        {
            var controllerPosition = vrController.transform.position;
            Debug.Assert(controllerPosition !=
                         Vector3.zero); // Basically, want to make sure we're getting something here

            numActiveColliders =
                Physics.OverlapSphereNonAlloc(controllerPosition, HitSphereRadius, activeColliders, layerMask);

            Debug.Log("Hit " + numActiveColliders + " things at " + controllerPosition);

            for (var i = 0; i < numActiveColliders; i++)
            {
                var activeCollider = activeColliders[i];

                Debug.Log("Hit object was at " + activeCollider.gameObject.transform.position);

                // Start having the game object follow this
                if (activeCollider.gameObject.TryGetComponent(out Follower follower))
                {
                    follower.StartFollowing(vrController);
                }
                else
                {
                    Debug.Log($"The handle won't follow the vr controller because we're missing the {nameof(Follower)} component.");
                }

                // And have things that should follow it start updating
                activeCollider.gameObject.GetComponent<FramePoserUpdater>().Updating = true;
            }
        }

        private void StopCurrentFollowers()
        {
            for (var i = 0; i < numActiveColliders; i++)
            {
                var activeCollider = activeColliders[i];

                if (activeCollider.gameObject.TryGetComponent(out Follower follower))
                {
                    follower.StopFollowing();
                }
                else
                {
                    Debug.Log($"Found an object, but it didn't have the {nameof(Follower)} component.");
                }

                activeCollider.gameObject.GetComponent<FramePoserUpdater>().Updating = false;
            }

            numActiveColliders = 0;
        }
    }
}