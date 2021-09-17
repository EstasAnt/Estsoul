using Assets.Scripts.Tools;
using Game.AI.CustomBehaviours.BlackboardData;
using Game.Movement;
using Tools.BehaviourTree;
using UnityEngine;

namespace Game.AI.CustomBehaviours.Tasks
{
    public class SimpleFlyToPointTask : BuildedTask
    {
        private MovementControllerBase _movementController;
        private MovementData _MovementData;

        private float _CurrentDirection;
        private float _LastChangeDirectionTime;

        public override void Init()
        {
            base.Init();
            _movementController = BehaviourTree.Executor.GetComponent<MovementControllerBase>();
            _MovementData = Blackboard.Get<MovementData>();
            _LastChangeDirectionTime = -_MovementData.DirectChangeCD;
        }

        public override TaskStatus Run()
        {
            _CurrentDirection = _movementController.Direction;
            if (!_MovementData.TargetPos.HasValue)
            {
                _movementController.SetHorizontal(0);
                _movementController.SetVertical(0);
                return TaskStatus.Failure;
            }
            var vectorToTarget = _MovementData.TargetPos.Value - _movementController.transform.position.ToVector2();
            var sqrDistToTarget = Vector2.SqrMagnitude(vectorToTarget);
            if (sqrDistToTarget <= _MovementData.ReachDistance * _MovementData.ReachDistance)
            {
                _movementController.SetHorizontal(0);
                _movementController.SetVertical(0);
                return TaskStatus.Success;
            }
            var directSign = Mathf.Sign(vectorToTarget.x);

            if (_CurrentDirection != directSign && Time.timeSinceLevelLoad - _LastChangeDirectionTime >= _MovementData.DirectChangeCD)
            {
                _movementController.SetHorizontal(directSign);
                _LastChangeDirectionTime = Time.timeSinceLevelLoad;
            }
            else
            {
                Vector2 vectorToTargetNormzd = vectorToTarget.normalized;
                _movementController.SetHorizontal(vectorToTargetNormzd.x);
                _movementController.SetVertical(vectorToTargetNormzd.y);
            }

            return TaskStatus.Running;
        }
    }
}
