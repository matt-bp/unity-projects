using System;
using System.Collections.Generic;
using UnityEngine;

namespace Prototypes._06_Quad_Handle.Scripts
{
    /// <summary>
    /// Makes game objects have the same world space positions as this object.
    /// </summary>
    public class LeaderGameObject : MonoBehaviour
    {
        public List<GameObject> followers;

        public List<Tuple<int, GameObject>> temp;
        // Start is called before the first frame update
        void Start()
        {
            // Calculate offsets?
        }

        // Update is called once per frame
        void Update()
        {
            // Need this to only happen when the simulation is stopped.
            // The follower needs to be something specific, like a cloth class, then I call a method to update its position.
            foreach (var follower in followers)
            {
                follower.transform.position = transform.position;
            }
        }
    }
}
