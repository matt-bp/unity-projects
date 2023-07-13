using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Prototypes._04_VR_Sphere_Follow.Scripts
{
    public class FollowTarget : MonoBehaviour
    {
        private Vector3 offset;
        private bool following = false;
        [CanBeNull] private Func<Vector3> getTargetPosition;

        public void StartFollowing(InputActionReference action)
        {
            following = true;
            getTargetPosition = () => action.action.ReadValue<Vector3>();
            offset = getTargetPosition() - transform.position;
        }
        
        public void StartFollowing(GameObject targetObject)
        {
            following = true;
            getTargetPosition = () => targetObject.transform.position;
            offset = getTargetPosition() - transform.position;
        }

        public void EndFollowing()
        {
            following = false;
        }

        private void Update()
        {
            if (!following) return;

            if (getTargetPosition is null) return;
            
            transform.position = getTargetPosition() - offset;
        }
    }
}