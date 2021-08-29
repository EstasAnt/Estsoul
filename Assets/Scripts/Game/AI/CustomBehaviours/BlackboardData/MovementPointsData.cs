using System;
using System.Collections.Generic;

namespace Game.AI.CustomBehaviours.BlackboardData
{
    [Serializable]
    public class MovementPointsData : global::BlackboardData
    {
        public List<MovementPointData> MovementPoints;
        public int CurrentMovePointIndex { get; set; }
        public float? StartWaitTimestamp { get; set; }
    }
}