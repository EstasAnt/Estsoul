using UnityDI;
using UnityEngine;

namespace Tools.BehaviourTree {

    public abstract class BehaviourTreeExecutor : MonoBehaviour {

        public BehaviourTree BehaviourTree { get; protected set; }

        protected virtual void Awake() {
            Initialize();
            BehaviourTree = BuildBehaviourTree();
            BehaviourTree.Blackboard = BuildBlackboard();
            BehaviourTree.Executor = this;
            BehaviourTree.Init();
        }

        protected virtual void Initialize()
        {
            ContainerHolder.Container.BuildUp(GetType(), this);
        }

        protected abstract BehaviourTree BuildBehaviourTree();

        protected abstract Blackboard BuildBlackboard();

        public virtual void UpdateBT() {
            BehaviourTree.UpdateTask();
        }

        public void SetBehaviour(BehaviourTree newTree) {
            BehaviourTree = newTree;
            BehaviourTree.Blackboard = BuildBlackboard();
            BehaviourTree.Executor = this;
            BehaviourTree.Init();
        }
    }
}