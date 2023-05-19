using UnityEngine;
using UnityEngine.InputSystem;

namespace Prototypes._06_Quad_Handle.Scripts
{
    public class FollowInputAction : MonoBehaviour
    {
        public InputActionReference actionToFollow;
        private Vector3 offset;
        private bool following;

        public void StartFollowing(InputActionReference action)
        {
            following = true;
            actionToFollow = action;
            offset = GetTargetPosition() - transform.position;
        }

        public void EndFollowing()
        {
            following = false;
        }

        private void Update()
        {
            if (!following) return;
            
            transform.position = GetTargetPosition() - offset;
        }

        private Vector3 GetTargetPosition() => actionToFollow.action.ReadValue<Vector3>();
    }
}