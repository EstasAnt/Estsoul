using Assets.Scripts.Tools;
using Game.AI.CustomBehaviours.BlackboardData;
using Game.Movement;
using Tools.BehaviourTree;
using UnityEngine;

namespace Game.AI.CustomBehaviours.Tasks
{
    public class SimpleMoveToPointTask : BuildedTask
    {
        private MovementControllerBase _movementController;
        private MovementData _MovementData;
        
        public override void Init()
        {
            base.Init();
            _movementController = BehaviourTree.Executor.GetComponent<MovementControllerBase>();
        }

        public override void Begin() {
            _MovementData = Blackboard.Get<MovementData>();
        }
        
        public override TaskStatus Run()
        {
            if (!_MovementData.TargetPos.HasValue)
            {
                _movementController.SetHorizontal(0);
                return TaskStatus.Failure;
            }
            var vectorToTarget = _MovementData.TargetPos.Value - _movementController.transform.position.ToVector2();
            var sqrDistToTarget = Vector2.SqrMagnitude(vectorToTarget);
            if (sqrDistToTarget <= _MovementData.ReachDistance * _MovementData.ReachDistance)
            {
                _movementController.SetHorizontal(0);
                return TaskStatus.Success;
            }
            _movementController.SetHorizontal(Mathf.Sign(vectorToTarget.x));
            
            return TaskStatus.Running;
        }
    }
}