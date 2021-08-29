using System.Collections;
using System.Collections.Generic;
using Game.AI.CustomBehaviours.BlackboardData;
using Game.AI.CustomBehaviours.Tasks;
using Tools.BehaviourTree;
using UnityDI;
using UnityEngine;

public class ZombieFrogAIBehaviour : BehaviourTreeExecutor
{
    public MovementPointsData MovementPointsData;
    protected override void Initialize()
    {
        ContainerHolder.Container.BuildUp(GetType(), this);
    }

    protected override BehaviourTree BuildBehaviourTree()
    {
        var behaviourTree = new BehaviourTree();
            var mainTree = behaviourTree.AddChild<ParallelTask>();
                mainTree.AddChild<PointPathSelectionTask>();
                mainTree.AddChild<SimpleMoveToPointTask>();
        return behaviourTree;
    }

    protected override Blackboard BuildBlackboard()
    {
        var bb = new Blackboard();
        bb.Set(MovementPointsData);
        return bb;
    }
    
    protected void Update() {
        //ToDo: Move update to scheduler service
        UpdateBT();
    }
}
