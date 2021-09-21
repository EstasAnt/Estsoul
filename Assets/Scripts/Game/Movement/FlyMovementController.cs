using System.Collections.Generic;
using Character.Health;
using Character.Movement.Modules;
using Game.Movement.Modules;
using Tools.BehaviourTree;
using UnityEngine;

namespace Game.Movement
{
    public class FlyMovementController : MovementControllerBase, IVelocityInheritor
    {
        [SerializeField]
        private FlyParameters FlyParameters;

        private FlyModule _FlyModule;


        public CharacterUnit Owner { get; private set; }
        public override float Horizontal => _FlyModule.Horizontal;

        public float Vertical => _FlyModule.Vertical;
        public override float Direction => _FlyModule.Direction;

        public override bool CanMove => true;

        protected override List<MovementModule> CreateModules()
        {
            var modules = new List<MovementModule>();

            _FlyModule = new FlyModule(FlyParameters);
            modules.Add(_FlyModule);
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
            commonData.Damageable = GetComponent<IDamageable>();
            _MovementModules.ForEach(_ => _.Initialize(_Blackboard));
        }

        public override void SetHorizontal(float hor)
        {
            _FlyModule.SetHorizontal(hor);
        }

        public override void SetVertical(float vertical)
        {
            _FlyModule.SetVertical(vertical);
        }

        private bool _Jumping = false;

        public override void SetDontMoveAnimationStateNames(List<string> stateNames)
        {
            _FlyModule.SetStopAnimatorStateNames(stateNames);
        }
    }
}
