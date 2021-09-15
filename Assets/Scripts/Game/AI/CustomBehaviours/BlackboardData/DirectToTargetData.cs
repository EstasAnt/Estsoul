using System;
using UnityEngine;

namespace Game.AI.CustomBehaviours.BlackboardData
{
    [Serializable]
    public class DirectToTargetData : global::BlackboardData
    {
        public Transform Root;
        public bool DirectSign;
        public float DirectCooldown;
    }
    
    
}