using System.Collections.Generic;
using System.Linq;
using Character.Health;
using Game.AI.CustomBehaviours.Behaviours;
using Game.AI.CustomBehaviours.BlackboardData;
using Game.Movement;
using Game.Movement.Enemies;
using Tools.BehaviourTree;
using UnityEngine;

namespace Game.AI.CustomBehaviours.Tasks
{
    public class TargetSearchTask : BuildedTask
    {

        private TargetSearchData _TargetSearchData;

        private EnemySimpleAnimationController _animationController;
        private MovementControllerBase _movementController;
        private IDamageable _damageable;
        
        public override void Init()
        {
            base.Init();
            _TargetSearchData = Blackboard.Get<TargetSearchData>();
            _animationController = BehaviourTree.Executor.GetComponentInChildren<EnemySimpleAnimationController>();
            _movementController = BehaviourTree.Executor.GetComponent<MovementControllerBase>();
            _damageable = BehaviourTree.Executor.GetComponent<IDamageable>();
            _damageable.OnDamage += DamageableOnOnDamage;
        }

        private void DamageableOnOnDamage(IDamageable arg1, Damage arg2)
        {
            _TargetSearchData.Target = CharacterUnit.Characters.FirstOrDefault();
            _TargetSearchData.LastTimeSawTargetTime = Time.timeSinceLevelLoad;
        }

        public override TaskStatus Run()
        {
            if (!(MonoBehaviour)_TargetSearchData.Target)
                _TargetSearchData.Target = null;
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
            
            _animationController.SetSeeTarget(_TargetSearchData.Target != null);

            if(_movementController != null)
                _movementController.MovementSpeedBoostCoef =
                _TargetSearchData.Target != null ? _TargetSearchData.HasTargetMoveCoef : 1f;
            
            return _TargetSearchData.Target == null ? TaskStatus.Failure : TaskStatus.Success;
        }
        

        private IMobsTarget TryFindTarget()
        {
            var foundColliders = new List<Collider2D>();
            var filter = new ContactFilter2D {useTriggers = false, layerMask = Layers.Masks.Character};
            var searchCollider = _TargetSearchData.TargetSearchTrigger != null ? _TargetSearchData.TargetSearchTrigger : _TargetSearchData.TargetAttackTrigger;
            if (searchCollider == null)
                return null;
            var foundCollidersCount = Physics2D.OverlapCollider(searchCollider, filter,
                foundColliders);
            if (foundCollidersCount == 0)
                return null;
            var mobsTargetCollider = foundColliders.FirstOrDefault(_ => _.GetComponent<IMobsTarget>() != null);
            return mobsTargetCollider == null ? null : mobsTargetCollider.GetComponent<IMobsTarget>();
        }
    }
}