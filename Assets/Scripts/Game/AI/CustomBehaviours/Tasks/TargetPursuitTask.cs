using Game.AI.CustomBehaviours.BlackboardData;
using Tools.BehaviourTree;

namespace Game.AI.CustomBehaviours.Tasks
{
    public class TargetPursuitTask : BuildedTask
    {
        private TargetSearchData _TargetSearchData;
        private MovementData _MovementData;
        
        public override void Init()
        {
            base.Init();
            _TargetSearchData = Blackboard.Get<TargetSearchData>();
            _MovementData = Blackboard.Get<MovementData>();
        }

        public override TaskStatus Run()
        {
            if (_TargetSearchData.Target == null)
                return TaskStatus.Failure;
            else
            {
                _MovementData.TargetPos = _TargetSearchData.Target.Transform.position;
                return TaskStatus.Success;
            }
        }
    }
}