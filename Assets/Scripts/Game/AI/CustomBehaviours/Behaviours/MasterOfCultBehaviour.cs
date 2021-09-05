using Character.Health;
using Game.AI.CustomBehaviours.BlackboardData;
using Game.AI.CustomBehaviours.Tasks;
using Tools.BehaviourTree;

namespace Game.AI.CustomBehaviours.Behaviours
{
    public class MasterOfCultBehaviour : BehaviourTreeExecutor
    {
        public TargetSearchData TargetSearchData;
        public DirectToTargetData DirectToTargetData;
        public AimToTargetData AimToTargetData;
        
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
            mainTree.AddChild<DirectToTargetTask>();
            mainTree.AddChild<AimToTargetTask>();
            mainTree.AddChild<AttackWeaponTargetTask>();
            return behaviourTree;
        }

        protected override Blackboard BuildBlackboard()
        {
            var bb = new Blackboard();
            bb.Set(TargetSearchData);
            bb.Set(DirectToTargetData);
            bb.Set(AimToTargetData);
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