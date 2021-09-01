using System.Linq;
using Character.Shooting;
using Game.AI.CustomBehaviours.BlackboardData;
using Game.Character.Melee;
using Tools.BehaviourTree;
using UnityEngine;

namespace Game.AI.CustomBehaviours.Tasks
{
    public class AttackMeleeWeaponTargetTask : BuildedTask
    {
        private WeaponController _WeaponController;
        private TargetSearchData _TargetSearchData;

        private MeleeWeapon _MeleeWeapon;
        
        public override void Init()
        {
            base.Init();
            _WeaponController = BehaviourTree.Executor.GetComponentInParent<WeaponController>();
            _TargetSearchData = Blackboard.Get<TargetSearchData>();
            _MeleeWeapon = _WeaponController.MainWeapon as MeleeWeapon;
        }

        public override TaskStatus Run()
        {
            if (_TargetSearchData.Target == null)
                return TaskStatus.Failure;
            var targetInAttackTrigger = CheckTargetInAttackTrigger();
            if (targetInAttackTrigger)
            {
                _WeaponController.PressFire();
                Debug.LogError("ZOMBIE ATTACK!");
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