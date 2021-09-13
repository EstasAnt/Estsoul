using Character.Shooting;
using Game.AI.CustomBehaviours.BlackboardData;
using Tools.BehaviourTree;
using UnityEngine;

namespace Game.AI.CustomBehaviours.Tasks
{
    public class AimToPointTask : BuildedTask
    {
        private TargetSearchData _TargetSearchData;
        private WeaponController _WeaponController;
        private AimToPointData _aimToTargetData;
        
        public override void Init()
        {
            base.Init();
            _TargetSearchData = Blackboard.Get<TargetSearchData>();
            _aimToTargetData = Blackboard.Get<AimToPointData>();
            _WeaponController = BehaviourTree.Executor.GetComponent<WeaponController>();
        }

        public override TaskStatus Run()
        {
            if (_TargetSearchData.Target == null)
                return TaskStatus.Failure;
            var aimPosition = _aimToTargetData.Point.position;
            _WeaponController.SetAimPosition(aimPosition);
            return TaskStatus.Success;
        }
    }
}