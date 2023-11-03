using UnityEngine;

namespace Ch3
{
    public class PlayerCharacter : MonoBehaviour
    {
        public void Hurt(int damage)
        {
            Managers.Player.ChangeHealth(-damage);
        }
    }
}