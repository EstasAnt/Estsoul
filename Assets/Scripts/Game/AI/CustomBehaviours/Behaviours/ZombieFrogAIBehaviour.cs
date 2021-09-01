using System;
using System.Collections;
using System.Collections.Generic;
using Character.Health;
using Character.Shooting;
using Game.AI.CustomBehaviours.BlackboardData;
using Game.AI.CustomBehaviours.Tasks;
using Game.Movement;
using Game.Weapons;
using Tools.BehaviourTree;
using UnityDI;
using UnityEngine;

public class ZombieFrogAIBehaviour : BehaviourTreeExecutor
{
    public MovementData MovementData;
    public MovementPointsData MovementPointsData;
    public TargetSearchData TargetSearchData;
    public SelectAttackData SelectAttackData;

    private IDamageable _Damageable;

    protected override void Awake()
    {
        base.Awake();
        _Damageable = GetComponent<IDamageable>();
    }

    protected override void Initialize()
    {
        ContainerHolder.Container.BuildUp(GetType(), this);
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
            mainTree.AddChild<SelectAttackTask>();
            mainTree.AddChild<AttackMeleeWeaponTargetTask>();

            return behaviourTree;
    }

    protected override Blackboard BuildBlackboard()
    {
        var bb = new Blackboard();
        bb.Set(MovementPointsData);
        bb.Set(TargetSearchData);
        bb.Set(MovementData);
        bb.Set(SelectAttackData);
        return bb;
    }
    
    protected void Update() {
        //ToDo: Move update to scheduler service
        if(_Damageable.Dead)
            return;
        UpdateBT();
    }
}
