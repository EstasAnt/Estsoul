using System;
using UnityEngine;

namespace Game.AI.CustomBehaviours.BlackboardData
{
    [Serializable]
    public class AimToPointData : global::BlackboardData
    {
        public Transform Point;
    }
}