using Game.AI.CustomBehaviours.BlackboardData;
using Tools.BehaviourTree;
using UnityEngine;

namespace Game.AI.CustomBehaviours.Tasks
{
    public class DirectToTargetTask : BuildedTask
    {
        private TargetSearchData _TargetSearchData;
        private DirectToTargetData _DirectToTargetData;

        private Vector3 _StartLocalScale;
        
        public override void Init()
        {
            base.Init();
            _TargetSearchData = Blackboard.Get<TargetSearchData>();
            _DirectToTargetData = Blackboard.Get<DirectToTargetData>();
            _StartLocalScale = _DirectToTargetData.Root.localScale;
        }
        
        public override TaskStatus Run()
        {
            if (_TargetSearchData.Target == null)
                return TaskStatus.Failure;
            var vectorToTarget =
                _TargetSearchData.Target.Transform.position - _DirectToTargetData.Root.position;
            var sign = Mathf.Sign(vectorToTarget.x);
            if (_DirectToTargetData.RotateSign)
                sign *= -1;
            _DirectToTargetData.Root.localScale =
                new Vector3(_StartLocalScale.x * sign, _StartLocalScale.y, _StartLocalScale.z);
            return TaskStatus.Success;
        }
    }
}