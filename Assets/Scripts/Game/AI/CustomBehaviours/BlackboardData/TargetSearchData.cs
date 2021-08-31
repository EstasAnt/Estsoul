using System;
using UnityEngine;

namespace Game.AI.CustomBehaviours.BlackboardData
{
    [Serializable]
    public class TargetSearchData : global::BlackboardData
    {
        public Collider2D TargetSearchTrigger;
        public float TryFoundMissedTargetTime = 5f;
        public float HasTargetMoveCoef = 1.5f;
        public Transform Target;
        public float LastTimeSawTargetTime;
    }
}