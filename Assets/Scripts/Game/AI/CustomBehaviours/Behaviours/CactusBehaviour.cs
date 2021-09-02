using Character.Health;
using Game.AI.CustomBehaviours.BlackboardData;
using Game.AI.CustomBehaviours.Tasks;
using Tools.BehaviourTree;

namespace Game.AI.CustomBehaviours.Behaviours
{
    public class CactusBehaviour : BehaviourTreeExecutor
    {
        
        public TargetSearchData TargetSearchData;
        
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
            mainTree.AddChild<AttackMeleeWeaponTargetTask>();
            return behaviourTree;
        }

        protected override Blackboard BuildBlackboard()
        {
            var bb = new Blackboard();
            bb.Set(TargetSearchData);
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