using UnityEngine;

namespace Devices
{
    public class BaseDevice : MonoBehaviour
    {
        public float radius = 3.5f;

        private void OnMouseUp()
        {
            var player = GameObject.FindWithTag("Player").transform;
            var playerPosition = player.position;

            playerPosition.y = transform.position.y;

            if (Vector3.Distance(transform.position, playerPosition) < radius)
            {
                var direction = transform.position - playerPosition;
                if (Vector3.Dot(player.forward, direction) > 0.5f)
                {
                    Operate();
                }
            }
        }
        
        public virtual void Operate() {}
    }
}