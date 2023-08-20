using System;
using UnityEngine;

namespace MassSpring.DataStructures
{
    [Serializable]
    public class ParticlePair
    {
        [SerializeField] public int firstIndex;
        [SerializeField] public int secondIndex;
    }
}