using Game.AI.CustomBehaviours.BlackboardData;
using Game.Movement;
using Tools.BehaviourTree;
using UnityEngine;

namespace Game.AI.CustomBehaviours.Tasks
{
    public class PointPathSelectionTask : BuildedTask
    {
        private MovementData _MovementData;
        private MovementPointsData _MovementPointsData;
        private MovementControllerBase _movementController;
        
        
        public override void Init()
        {
            base.Init();
            _movementController = BehaviourTree.Executor.GetComponent<MovementControllerBase>();
        }
        
        public override void Begin() {
            _MovementData = Blackboard.Get<MovementData>();
            _MovementPointsData = Blackboard.Get<MovementPointsData>();
            _MovementPointsData.CurrentMovePointIndex = 0;
        }
        
        public override TaskStatus Run()
        {
            if (_MovementPointsData.MovementPoints.Count <= _MovementPointsData.CurrentMovePointIndex)
                return TaskStatus.Failure;
            var currentTargetPoint = _MovementPointsData.MovementPoints[_MovementPointsData.CurrentMovePointIndex];

            var dist = Vector2.Distance(_movementController.transform.position,
                currentTargetPoint.PointTransform.position);
            if (dist <= _MovementData.ReachDistance)
            {
                if (currentTargetPoint.WaitTime > 0f)
                {
                    if (!_MovementPointsData.StartWaitTimestamp.HasValue)
                    {
                        _MovementPointsData.StartWaitTimestamp = Time.timeSinceLevelLoad;
                    }
                    else
                    {
                        if (Time.timeSinceLevelLoad - _MovementPointsData.StartWaitTimestamp >
                            currentTargetPoint.WaitTime)
                        {
                            _MovementPointsData.CurrentMovePointIndex = (_MovementPointsData.CurrentMovePointIndex + 1) % _MovementPointsData
                                .MovementPoints.Count;
                            _MovementPointsData.StartWaitTimestamp = null;
                        }
                    }    
                }
                else
                {
                    _MovementPointsData.CurrentMovePointIndex = (_MovementPointsData.CurrentMovePointIndex + 1) % _MovementPointsData
                        .MovementPoints.Count;
                }
            }
            else
            {
                _MovementData.TargetPos = currentTargetPoint.PointTransform.position;
            }
            return TaskStatus.Running;
        }
        
        
    }
}