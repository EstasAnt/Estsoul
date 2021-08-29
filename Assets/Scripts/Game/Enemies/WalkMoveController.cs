using System.Collections.Generic;
using Character.Movement;
using Character.Movement.Modules;
using Game.Movement;
using Game.Movement.Modules;
using Tools.BehaviourTree;
using UnityEngine;

namespace Game.Movement.Enemies
{
    public class WalkMoveController : MovementControllerBase
    {
        [SerializeField]
        private WalkParameters WalkParameters;
        [SerializeField]
        private GroundCheckParameters GroundCheckParameters;
        
        public override float Horizontal { get; }
        
        private WalkModule _WalkModule;
        private GroundCheckModule _GroundCheckModule;
        protected override List<MovementModule> CreateModules()
        {
            var modules = new List<MovementModule>();
            _WalkModule = new WalkModule(WalkParameters);
            _GroundCheckModule = new GroundCheckModule(GroundCheckParameters);
            return modules;
        }

        protected override void SetupBlackboard()
        {
            _Blackboard = new Blackboard();
            var commonData = _Blackboard.Get<CommonData>();
            commonData.ObjRigidbody = Rigidbody;
            commonData.ObjTransform = this.transform;
            commonData.BodyCollider = BodyCollider;
            commonData.GroundCollider = GroundCollider;
            commonData.MovementController = this;
            _MovementModules.ForEach(_ => _.Initialize(_Blackboard));
        }

        public override void SetHorizontal(float hor)
        {
            _WalkModule.SetHorizontal(hor);
        }

        public override void SetVertical(float vertical)
        {
            _WalkModule.SetVertical(vertical);
        }
    }
}