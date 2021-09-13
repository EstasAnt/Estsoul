using Character.Health;
using Game.AI.CustomBehaviours.BlackboardData;
using Game.AI.CustomBehaviours.Tasks;
using Tools.BehaviourTree;

namespace Game.AI.CustomBehaviours.Behaviours
{
    public class PlyvakaBehaviour : BehaviourTreeExecutor
    {
        
        public TargetSearchData TargetSearchData;
        public AimToPointData AimToPointData;
        
        private IDamageable _Damageable;
        
        protected override void Awake()
        {
            base.Awake();
            _Damageable = GetComponent<IDamageable>();
        }

        protected override BehaviourTree BuildBehaviourTree()
        {
            var behaviourTree = new BehaviourTree();
            var mainTree = behaviourTree.AddChild<ParallelTask>();
            mainTree.AddChild<TargetSearchTask>();
            mainTree.AddChild<AimToPointTask>();
            mainTree.AddChild<AttackWeaponTargetTask>();
            return behaviourTree;
        }

        protected override Blackboard BuildBlackboard()
        {
            var bb = new Blackboard();
            bb.Set(TargetSearchData);
            bb.Set(AimToPointData);
            return bb;
        }
        
        protected void Update() {
            //ToDo: Move update to scheduler service
            if(_Damageable.Dead)
                return;
            UpdateBT();
        }
    }
}