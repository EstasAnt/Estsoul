using System.Collections.Generic;
using System.Linq;
using Game.AI.CustomBehaviours.Behaviours;
using Game.AI.CustomBehaviours.BlackboardData;
using Tools.BehaviourTree;
using UnityEngine;

namespace Game.AI.CustomBehaviours.Tasks
{
    public class TargetSearchTask : BuildedTask
    {

        private TargetSearchData _TargetSearchData;

        public override void Init()
        {
            base.Init();
            _TargetSearchData = Blackboard.Get<TargetSearchData>();
        }

        public override TaskStatus Run()
        {
            var foundTarget = TryFindTarget();
            
            if (foundTarget == null)
            {
                if (Time.timeSinceLevelLoad - _TargetSearchData.LastTimeSawTargetTime >
                    _TargetSearchData.TryFoundMissedTargetTime)
                {
                    _TargetSearchData.Target = null;
                }
            }
            else
            {
                _TargetSearchData.Target = foundTarget;
            }
            
            if(foundTarget != null)
                _TargetSearchData.LastTimeSawTargetTime = Time.timeSinceLevelLoad;
            
            return _TargetSearchData.Target == null ? TaskStatus.Failure : TaskStatus.Success;
        }

        private Transform TryFindTarget()
        {
            var foundColliders = new List<Collider2D>();
            var filter = new ContactFilter2D {useTriggers = false, layerMask = Layers.Masks.Character};
            var foundCollidersCount = Physics2D.OverlapCollider(_TargetSearchData.TargetSearchTrigger, filter,
                foundColliders);
            if (foundCollidersCount == 0)
                return null;
            var mobsTargetCollider = foundColliders.FirstOrDefault(_ => _.GetComponent<IMobsTarget>() != null);


            return mobsTargetCollider == null ? null : mobsTargetCollider.transform;
        }
    }
}