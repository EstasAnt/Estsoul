using Game.AI.CustomBehaviours.BlackboardData;
using Tools.BehaviourTree;
using UnityEngine;

namespace Game.AI.CustomBehaviours.Tasks
{
    public class DirectToTargetTask : BuildedTask
    {
        private TargetSearchData _TargetSearchData;
        private DirectToTargetData _DirectToTargetData;
        private Animator _Animator;

        private Vector3 _StartLocalScale;

        private float _LastDirectTime;
        
        public override void Init()
        {
            base.Init();
            _Animator = BehaviourTree.Executor.GetComponentInChildren<Animator>();
            _TargetSearchData = Blackboard.Get<TargetSearchData>();
            _DirectToTargetData = Blackboard.Get<DirectToTargetData>();
            _StartLocalScale = _DirectToTargetData.Root.localScale;
        }
        
        public override TaskStatus Run()
        {
            if (_TargetSearchData.Target == null)
                return TaskStatus.Failure;
            if (Time.timeSinceLevelLoad - _LastDirectTime < _DirectToTargetData.DirectCooldown)
                return TaskStatus.Running;
            var clipInfo = _Animator.GetCurrentAnimatorClipInfo(0);
            if (clipInfo[0].clip.name.Contains("Attack"))
                return TaskStatus.Running;
            var vectorToTarget =
                _TargetSearchData.Target.Transform.position - _DirectToTargetData.Root.position;
            var sign = Mathf.Sign(vectorToTarget.x);
            var currentSign = Mathf.Sign(_DirectToTargetData.Root.localScale.x);
            if (_DirectToTargetData.DirectSign)
                sign *= -1;
            _DirectToTargetData.Root.localScale =
                new Vector3(_StartLocalScale.x * sign, _StartLocalScale.y, _StartLocalScale.z);
            if (Mathf.Sign(_DirectToTargetData.Root.localScale.x) != currentSign)
                _LastDirectTime = Time.timeSinceLevelLoad;
            return TaskStatus.Success;
        }
    }
}