using System;
using UnityEngine;

namespace Game.AI.CustomBehaviours.BlackboardData
{
    [Serializable]
    public class MovementData : global::BlackboardData
    {
        public Vector2? TargetPos { get; set; }
        public float ReachDistance = 0.1f;
    }
}