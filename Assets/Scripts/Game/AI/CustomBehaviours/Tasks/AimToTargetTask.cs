using Character.Shooting;
using Game.AI.CustomBehaviours.BlackboardData;
using Tools.BehaviourTree;
using UnityEngine;

namespace Game.AI.CustomBehaviours.Tasks
{
    public class AimToTargetTask : BuildedTask
    {
        private TargetSearchData _TargetSearchData;
        private WeaponController _WeaponController;
        private AimToTargetData _aimToTargetData;
        
        public override void Init()
        {
            base.Init();
            _TargetSearchData = Blackboard.Get<TargetSearchData>();
            _aimToTargetData = Blackboard.Get<AimToTargetData>();
            _WeaponController = BehaviourTree.Executor.GetComponent<WeaponController>();
        }

        public override TaskStatus Run()
        {
            if (_TargetSearchData.Target == null)
                return TaskStatus.Failure;
            var aimPosition = _TargetSearchData.Target.Collider.transform.position + Vector3.up * _aimToTargetData.OffsetY;
            _WeaponController.SetAimPosition(aimPosition);
            return TaskStatus.Success;
        }
    }
}