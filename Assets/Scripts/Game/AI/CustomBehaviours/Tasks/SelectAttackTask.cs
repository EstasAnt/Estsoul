using Assets.Scripts.Tools;
using Character.Shooting;
using Game.AI.CustomBehaviours.BlackboardData;
using Game.Character.Melee;
using Game.Movement;
using Tools.BehaviourTree;
using UnityEngine;

namespace Game.AI.CustomBehaviours.Tasks
{
    public class SelectAttackTask : BuildedTask
    {
        private MovementControllerBase _movementController;
        private WeaponController _WeaponController;
        private TargetSearchData _TargetSearchData;
        private MeleeWeapon _MeleeWeapon;
        private SelectAttackData _SelectAttackData;
        private MovementData _MovementData;
        
        public override void Init()
        {
            base.Init();
            _movementController = BehaviourTree.Executor.GetComponent<MovementControllerBase>();
            _WeaponController = BehaviourTree.Executor.GetComponentInParent<WeaponController>();
            _TargetSearchData = Blackboard.Get<TargetSearchData>();
            _SelectAttackData = Blackboard.Get<SelectAttackData>();
            _MovementData = Blackboard.Get<MovementData>();
            _MeleeWeapon = _WeaponController.MainWeapon as MeleeWeapon;
        }
        
        public override TaskStatus Run()
        {
            if (_TargetSearchData.Target != null && _MovementData.TargetPos.HasValue)
            {
                var vectorToTarget = _MovementData.TargetPos.Value - _movementController.transform.position.ToVector2();
                var sqrDistToTarget = Vector2.SqrMagnitude(vectorToTarget);
                if (sqrDistToTarget <= _SelectAttackData.MinDistanceToJumpAttack * _SelectAttackData.MinDistanceToJumpAttack)
                {
                    _MeleeWeapon.SelectAttackGroup(0);
                }
                else
                {
                    _MeleeWeapon.SelectAttackGroup(1);
                }
            }
            else
            {
                _MeleeWeapon.SelectAttackGroup(0);
            }
                return TaskStatus.Running;
        }
    }
}