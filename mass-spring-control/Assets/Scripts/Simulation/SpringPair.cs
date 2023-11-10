using System;
using UnityEngine;

namespace Simulation
{
    [Serializable]
    public class SpringPair
    {
        [SerializeField] public int firstIndex;
        [SerializeField] public int secondIndex;
    }
}
