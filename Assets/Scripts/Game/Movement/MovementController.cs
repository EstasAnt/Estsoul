﻿using System;
using System.Collections.Generic;
using Character.Health;
using Character.Movement.Modules;
using Game.Movement.Modules;
using Tools.BehaviourTree;
using UnityEngine;

namespace Game.Movement {
    public class MovementController : MovementControllerBase, IVelocityInheritor {
        [SerializeField]
        private WalkParameters WalkParameters;
        [SerializeField]
        private GroundCheckParameters GroundCheckParameters;
        [SerializeField]
        private WallCheckParameters WallCheckParameters;
        [SerializeField]
        private WallsSlideParameters WallSlideParameters;
        [SerializeField]
        private LedgeHangParameters LedgeHangParameters;
        [SerializeField]
        private PushingParameters PushingParameters;
        [SerializeField]
        private JumpParameters JumpParameters;
        [SerializeField] 
        private RollParameters RollParameters;
        [SerializeField]
        private OneWayPlatformParameters OneWayPlatformParameters;
        
        private WalkModule _WalkModule;
        private GroundCheckModule _GroundCheckModule;
        private WallsCheckModule _WallsCheckModule;
        private WallsSlideModule _WallsSlideModule;
        private JumpModule _JumpModule;
        private RollModule _RollModule;
        
        private LedgeHangModule _LedgeHangModule;
        private PushingModule _PushingModule;
        private OneWayPlatformModule _OneWayPlatformModule;
        public CharacterUnit Owner { get; private set; }

        public event Action<string> RollAnimationEvent; 
        
        public override bool IsGrounded => _GroundCheckModule.IsGrounded;
        public bool IsMainGrounded => _GroundCheckModule.IsMainGrounded;
        public float MinDistanceToGround => _GroundCheckModule.MinDistanceToGround;
        public bool FallingDown => _GroundCheckModule.FallingDown;
        public bool WallSliding => _WallsSlideModule.WallSliding;
        public bool WallRun => _WallsSlideModule.WallRun;
        public bool LedgeHang => _LedgeHangModule.LedgeHang;
        public bool Pushing => _PushingModule.Pushing;
        public float TimeFallingDown => _GroundCheckModule.TimeFallingDown;
        public float TimeNotFallingDown => _GroundCheckModule.TimeNotFallingDown;

        public bool DoubleJump => _JumpModule.DoubleJump;

        public float MaxSpeed => WalkParameters.Speed;

        public PhysicsMaterial2D BodyColliderDefaultPhysicsMaterial { get; private set; }

        public override float Horizontal => _WalkModule.Horizontal;
        public override float Direction => _WalkModule.Direction;

        public override bool CanMove => !_LedgeHangModule.LedgeHang;

        protected override void Awake() {
            base.Awake();
            Owner = GetComponent<CharacterUnit>();
            BodyColliderDefaultPhysicsMaterial = BodyCollider.sharedMaterial;
            GroundCheckParameters.GroundSensors.ForEach(_ => {
                _.AddExtentionCollider(GroundCollider);
                _.AddExtentionCollider(BodyCollider);
            });
            GroundCheckParameters.MainGroundSensors.ForEach(_ => {
                _.AddExtentionCollider(GroundCollider);
                _.AddExtentionCollider(BodyCollider);
            });
        }

        protected override void Start()
        {
            base.Start();
            _RollModule.AnimationTriggerEvent += RollModuleOnAnimationTriggerEvent;
        }

        private void RollModuleOnAnimationTriggerEvent(string obj)
        {
            RollAnimationEvent?.Invoke(obj);
        }

        protected override List<MovementModule> CreateModules() {
            var modules = new List<MovementModule>();

            _WalkModule = new WalkModule(WalkParameters);
            _GroundCheckModule = new GroundCheckModule(GroundCheckParameters);
            _WallsCheckModule = new WallsCheckModule(WallCheckParameters);
            _WallsSlideModule = new WallsSlideModule(WallSlideParameters);
            _JumpModule = new JumpModule(JumpParameters);
            _RollModule = new RollModule(RollParameters);
            _LedgeHangModule = new LedgeHangModule(LedgeHangParameters);
            _PushingModule = new PushingModule(PushingParameters);
            _OneWayPlatformModule = new OneWayPlatformModule(OneWayPlatformParameters);

            modules.Add(_GroundCheckModule);
            modules.Add(_WallsCheckModule);
            modules.Add(_WalkModule);
            modules.Add(_LedgeHangModule);
            modules.Add(_WallsSlideModule);
            modules.Add(_JumpModule);
            modules.Add(_RollModule);
            modules.Add(_PushingModule);
            modules.Add(_OneWayPlatformModule);
            return modules;
        }

        protected override void SetupBlackboard() {
            _Blackboard = new Blackboard();
            var commonData = _Blackboard.Get<CommonData>();
            commonData.ObjRigidbody = Rigidbody;
            commonData.ObjTransform = this.transform;
            commonData.BodyCollider = BodyCollider;
            commonData.GroundCollider = GroundCollider;
            commonData.MovementController = this;
            commonData.IDamageable = GetComponent<IDamageable>();
            _MovementModules.ForEach(_ => _.Initialize(_Blackboard));
        }

        protected override void LateUpdate() {
            base.LateUpdate();
            _JumpHold = false;
        }

        protected override void FixedUpdate() {
            base.FixedUpdate();
            _MovementModules.ForEach(_ => _.FixedUpdate());
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if(_RollModule != null)
                _RollModule.AnimationTriggerEvent -= RollModuleOnAnimationTriggerEvent;
        }

        public override void SetHorizontal(float hor) {
            _WalkModule.SetHorizontal(hor);
        }

        public override void SetVertical(float vertical) {
            _WalkModule.SetVertical(vertical);
        }

        private bool _Jumping = false;

        public bool Jump() {
            _Jumping = _JumpModule.Jump(this);
            if (!_Jumping)
                _Jumping = _JumpModule.AirJump(this);
            return _Jumping;
        }

        public bool WallJump() {
            return _JumpModule.WallJump(this);
        }

        public bool Roll()
        {
            _RollModule.Roll();
            return true;
        }
        
        public bool FallDownPlatform() {
            return _OneWayPlatformModule.FallDownPlatform();
        }

        public void SetCanLegeHang(bool val)
        {
            _LedgeHangModule.CanLedgeHang = val;
        }

        public override void PlayMoveSound()
        {
            base.PlayMoveSound();
            _WalkModule.ProcessRunSound();
        }
    }
}
