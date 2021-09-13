using System.Linq;
using Character.Shooting;
using Game.AI.CustomBehaviours.BlackboardData;
using Game.Character.Melee;
using Tools.BehaviourTree;
using UnityEngine;

namespace Game.AI.CustomBehaviours.Tasks
{
    public class AttackWeaponTargetTask : BuildedTask
    {
        private WeaponController _WeaponController;
        private TargetSearchData _TargetSearchData;

        public override void Init()
        {
            base.Init();
            _WeaponController = BehaviourTree.Executor.GetComponentInParent<WeaponController>();
            _TargetSearchData = Blackboard.Get<TargetSearchData>();
        }

        public override TaskStatus Run()
        {
            if (_TargetSearchData.Target == null)
                return TaskStatus.Failure;
            var targetInAttackTrigger = CheckTargetInAttackTrigger();
            if (targetInAttackTrigger)
            {
                _WeaponController.PressFire();
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }

        private bool CheckTargetInAttackTrigger()
        {
            var targetCollider = _TargetSearchData.Target.Collider;
            return _TargetSearchData.TargetAttackTrigger.IsTouching(targetCollider, new ContactFilter2D() {useTriggers = true });
        }
    }
}