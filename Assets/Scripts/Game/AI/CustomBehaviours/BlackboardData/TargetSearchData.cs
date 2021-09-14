using System;
using Character.Health;
using Game.AI.CustomBehaviours.Behaviours;
using UnityEngine;

namespace Game.AI.CustomBehaviours.BlackboardData
{
    [Serializable]
    public class TargetSearchData : global::BlackboardData
    {
        public Collider2D TargetSearchTrigger;
        public Collider2D TargetAttackTrigger;
        public float TryFoundMissedTargetTime = 5f;
        public float HasTargetMoveCoef = 1.5f;
        public IDamageable Target;
        public float LastTimeSawTargetTime;
    }
}