using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Prototypes._04_VR_Sphere_Follow.Scripts
{
    public class FollowTarget : MonoBehaviour
    {
        public InputActionReference actionToFollow;
        private Vector3 offset;
        private bool following = false;

        public void OnFollowStart(InputActionReference action)
        {
            following = true;
            Debug.Log("Started following!");
            actionToFollow = action;
            offset = GetTargetPosition() - transform.position;
        }

        private void Update()
        {
            if (!following) return;
            
            transform.position = GetTargetPosition() - offset;
        }

        private Vector3 GetTargetPosition() => actionToFollow.action.ReadValue<Vector3>();
    }
}