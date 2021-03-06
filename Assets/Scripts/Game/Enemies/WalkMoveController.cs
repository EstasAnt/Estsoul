using System.Collections.Generic;
using Character.Health;
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
        
        public override float Horizontal => _WalkModule.Horizontal;
        
        public override float Direction => _WalkModule.Direction;
        
        private WalkModule _WalkModule;
        private GroundCheckModule _GroundCheckModule;

        private IDamageable _Damageable;

        public override bool CanMove => true;

        protected override void Awake()
        {
            base.Awake();
            _Damageable = GetComponent<IDamageable>();
            if (_Damageable != null)
                _Damageable.OnKill += DamageableOnOnKill;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (_Damageable != null)
                _Damageable.OnKill -= DamageableOnOnKill;
        }

        private void DamageableOnOnKill(IDamageable arg1, Damage arg2)
        {
            _WalkModule.SetHorizontal(0);
        }

        protected override List<MovementModule> CreateModules()
        {
            var modules = new List<MovementModule>();
            _WalkModule = new WalkModule(WalkParameters);
            _GroundCheckModule = new GroundCheckModule(GroundCheckParameters);
            
            modules.Add(_GroundCheckModule);
            modules.Add(_WalkModule);
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
        
        public override void SetDontMoveAnimationStateNames(List<string> stateNames)
        {
            _WalkModule.SetStopAnimatorStateNames(stateNames);
        }
    }
}