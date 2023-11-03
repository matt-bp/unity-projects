using UnityEngine;

namespace Ch3
{
    public class Fireball : MonoBehaviour
    {
        public float speed = 10.0f;
        public int damage = 1;

        private void Update()
        {
            transform.Translate(0, 0, speed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            var player = other.GetComponent<PlayerCharacter>();

            if (player != null)
            {
                player.Hurt(damage);
            }

            Destroy(gameObject);
        }
    }
}