using System;
using UnityEngine;

namespace Game.AI.CustomBehaviours.BlackboardData
{
    [Serializable]
    public class MovementPointData
    {
        public Transform PointTransform;
        public float WaitTime;
    }
}