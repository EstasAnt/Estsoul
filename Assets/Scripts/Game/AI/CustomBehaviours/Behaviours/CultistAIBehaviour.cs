using Character.Health;
using Game.AI.CustomBehaviours.BlackboardData;
using Game.AI.CustomBehaviours.Tasks;
using Tools.BehaviourTree;

public class CultistAIBehaviour : BehaviourTreeExecutor
{
    public MovementData MovementData;
    public MovementPointsData MovementPointsData;
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
            
        var moveSelector = mainTree.AddChild<SelectorTask>();
        moveSelector.AddChild<TargetPursuitTask>();
        moveSelector.AddChild<PointPathSelectionTask>();
        mainTree.AddChild<SimpleMoveToPointTask>();
        mainTree.AddChild<AttackWeaponTargetTask>();

        return behaviourTree;
    }

    protected override Blackboard BuildBlackboard()
    {
        var bb = new Blackboard();
        bb.Set(MovementPointsData);
        bb.Set(TargetSearchData);
        bb.Set(MovementData);
        return bb;
    }
    
    protected void Update() {
        //ToDo: Move update to scheduler service
        if(_Damageable.Dead)
            return;
        UpdateBT();
    }
}