using UnityEngine;

namespace Game.AI.CustomBehaviours.BlackboardData
{
    public class MovementData : global::BlackboardData
    {
        public Vector2? TargetPos;
        public float ReachDistance = 0.1f;
    }
}