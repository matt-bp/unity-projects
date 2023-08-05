using System;
using UnityEngine;

namespace MassSpring
{
    [Serializable]
    public class ParticlePair
    {
        [SerializeField] public int firstIndex;
        [SerializeField] public int secondIndex;
    }
}