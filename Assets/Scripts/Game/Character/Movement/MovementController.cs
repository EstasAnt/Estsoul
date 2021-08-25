﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Character.Control;
using Character.Movement.Modules;
using Character.Shooting;
using Core.Services.Game;
using Tools.BehaviourTree;
using UnityEditor;
using UnityEngine;

namespace Character.Movement {
    public class MovementController : MonoBehaviour, IVelocityInheritor {
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
        private OneWayPlatformParameters OneWayPlatformParameters;

        public Collider2D GroundCollider;
        public Collider2D BodyCollider;

        private List<MovementModule> _MovementModules;
        private WalkModule _WalkModule;
        private GroundCheckModule _GroundCheckModule;
        private WallsCheckModule _WallsCheckModule;
        private WallsSlideModule _WallsSlideModule;
        private JumpModule _JumpModule;
        private LedgeHangModule _LedgeHangModule;
        private PushingModule _PushingModule;
        private OneWayPlatformModule _OneWayPlatformModule;

        private Blackboard _Blackboard;


        private WalkData _WalkData;

        public CharacterUnit Owner { get; private set; }
        public Rigidbody2D Rigidbody { get; private set; }
        //public Collider2D Collider { get; private set; }

        public Vector2 Velocity => Rigidbody.velocity;
        public Vector2 LocalVelocity { get; private set; }

        public float Horizontal => _WalkModule.Horizontal;
        public bool IsGrounded => _GroundCheckModule.IsGrounded;
        public bool IsMainGrounded => _GroundCheckModule.IsMainGrounded;
        public float MinDistanceToGround => _GroundCheckModule.MinDistanceToGround;
        public bool FallingDown => _GroundCheckModule.FallingDown;
        public bool WallSliding => _WallsSlideModule.WallSliding;
        public float Direction => _WalkModule.Direction;
        public bool WallRun => _WallsSlideModule.WallRun;
        public bool LedgeHang => _LedgeHangModule.LedgeHang;
        public bool Pushing => _PushingModule.Pushing;
        public float TimeFallingDown => _GroundCheckModule.TimeFallingDown;
        public float TimeNotFallingDown => _GroundCheckModule.TimeNotFallingDown;

        public float OverridedAirAcceleration { get; set; }
        public bool OverrideAirAcceleration { get; set; }

        public Rigidbody2D AttachedToRB { get; set; }
        public bool CanDetach { get; set; }

        public event Action OnPressJump;
        public event Action OnHoldJump;
        public event Action OnReleaseJump;

        public PhysicsMaterial2D BodyColliderDefaultPhysicsMaterial { get; private set; }

        private void Awake() {
            Owner = GetComponent<CharacterUnit>();
            Rigidbody = GetComponent<Rigidbody2D>();
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

        private void Start() {
            InitializeModules();
            SetupBlackboard();
            SetupCommon();
            _WalkData = _Blackboard.Get<WalkData>();
            _MovementModules.ForEach(_ => _.Start());
        }

        private void InitializeModules() {
            _MovementModules = new List<MovementModule>();

            _WalkModule = new WalkModule(WalkParameters);
            _GroundCheckModule = new GroundCheckModule(GroundCheckParameters);
            _WallsCheckModule = new WallsCheckModule(WallCheckParameters);
            _WallsSlideModule = new WallsSlideModule(WallSlideParameters);
            _JumpModule = new JumpModule(JumpParameters);
            _LedgeHangModule = new LedgeHangModule(LedgeHangParameters);
            _PushingModule = new PushingModule(PushingParameters);
            _OneWayPlatformModule = new OneWayPlatformModule(OneWayPlatformParameters);

            _MovementModules.Add(_GroundCheckModule);
            _MovementModules.Add(_WallsCheckModule);
            _MovementModules.Add(_WalkModule);
            _MovementModules.Add(_LedgeHangModule);
            _MovementModules.Add(_WallsSlideModule);
            _MovementModules.Add(_JumpModule);
            _MovementModules.Add(_PushingModule);
            _MovementModules.Add(_OneWayPlatformModule);
        }

        private void SetupBlackboard() {
            _Blackboard = new Blackboard();
            _MovementModules.ForEach(_ => _.Initialize(_Blackboard));
        }

        private void SetupCommon() {
            var commonData = _Blackboard.Get<CommonData>();
            commonData.ObjRigidbody = Rigidbody;
            commonData.ObjTransform = this.transform;
            commonData.BodyCollider = BodyCollider;
            commonData.GroundCollider = GroundCollider;
            commonData.MovementController = this;
            commonData.WeaponController = Owner.WeaponController;
        }
        
        private bool _JumpHold;

        private void Update() {
            _MovementModules.ForEach(_ => _.Update());
            LocalVelocity = Velocity;
            if (!AttachedToRB) {
                AttachedToRB = null;
                CanDetach = true;
            } else {
                LocalVelocity -= AttachedToRB.velocity;
            }
        }

        private void LateUpdate() {
            _MovementModules.ForEach(_ => _.LateUpdate());
            _JumpHold = false;
        }

        private void FixedUpdate() {
            _MovementModules.ForEach(_ => _.FixedUpdate());
        }

        private void OnCollisionEnter2D(Collision2D collision) {
            _MovementModules.ForEach(_ => _.OnCollisionEnter2D(collision));
            // if(CanDetach && AttachedToRB && Layers.Masks.LayerInMask(Layers.Masks.GroundAndPlatform, collision.gameObject.layer)) {
            //     AttachedToRB = null;
            // }
        }

        private void OnCollisionExit2D(Collision2D collision) {
            _MovementModules.ForEach(_ => _.OnCollisionExit2D(collision));
        }

        public void SetHorizontal(float hor) {
            _WalkModule.SetHorizontal(hor);
        }

        public void SetVertical(float vertical) {
            _WalkModule.SetVertical(vertical);
        }

        private bool _Jumping = false;

        public bool Jump() {
            if (Owner.WeaponController.HasVehicle && Owner.WeaponController.Vehicle.InputProcessor.CurrentMagazine != 0)
                return false;
            _Jumping = _JumpModule.Jump(this);
            if (!_Jumping)
                _Jumping = _JumpModule.AirJump(this);
            return _Jumping;
        }

        public bool WallJump() {
            if (Owner.WeaponController.HasVehicle && Owner.WeaponController.Vehicle.InputProcessor.CurrentMagazine != 0)
                return false;
            return _JumpModule.WallJump(this);
        }

        public void ChangeDirection(int newDir) {
            if (_WalkData.Direction == newDir) //_WalkData.Direction == newDir
                return;
            _WalkData.Direction = newDir;
            var localScale = WalkParameters.IkTransform.localScale;
            var newLocalScale = new Vector3(newDir * Mathf.Abs(localScale.x), localScale.y, localScale.z);
            WalkParameters.IkTransform.localScale = newLocalScale;
        }

        public bool FallDownPlatform() {
            return _OneWayPlatformModule.FallDownPlatform();
        }

        public void SubscribeWeaponOnEvents(Weapon weapon) {
            OnHoldJump += weapon.InputProcessor.ProcessHold;
            OnPressJump += weapon.InputProcessor.ProcessPress;
            OnReleaseJump += weapon.InputProcessor.ProcessRelease;
        }

        public void UnSubscribeWeaponOnEvents(Weapon weapon) {
            OnHoldJump -= weapon.InputProcessor.ProcessHold;
            OnPressJump -= weapon.InputProcessor.ProcessPress;
            OnReleaseJump -= weapon.InputProcessor.ProcessRelease;
        }

        public bool ProcessHoldJump() {
            _JumpHold = true;
            OnHoldJump?.Invoke();
            return false;
        }

        public void PressJump() {
            OnPressJump?.Invoke();
        }

        public void ReleaseJump() {
            OnReleaseJump?.Invoke();
        }

    }
}
