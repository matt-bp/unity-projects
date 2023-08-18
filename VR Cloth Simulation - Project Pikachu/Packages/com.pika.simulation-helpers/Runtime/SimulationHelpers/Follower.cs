using System;
using UnityEngine;

namespace SimulationHelpers
{
    /// <summary>
    /// This makes the attached game object follow another game object (its leader).
    ///
    /// It does not remember who its previous leader was.
    /// </summary>
    [AddComponentMenu("Simulation Helpers/Follower")]
    public class Follower : MonoBehaviour
    {
        private Vector3 offset;
        private GameObject leader;

        public void StartFollowing(GameObject newLeader)
        {
            leader = newLeader;
            offset = leader.transform.position - transform.position;
        }

        public void StopFollowing()
        {
            leader = null;
        }

        private void Update()
        {
            if (leader is null) return;

            transform.position = leader.transform.position - offset;
        }
    }
}