﻿using Core.Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Character.Melee;
using Tools.BehaviourTree;
using UnityDI;
using UnityEngine;

namespace Character.Movement.Modules {
    public class WalkModule : MovementModule {
        [Dependency]
        private readonly AudioService _AudioService;

        public float Direction => _WalkData.Direction;

        private GroundedData _GroundedData;
        private WallSlideData _WallSlideData;
        private WalkData _WalkData;

        private CharacterAnimationController _characterAnimation;
        
        private WalkParameters _Parameters;
        private float _TargetXVelocity = 0f;
        public float Horizontal => _WalkData.Horizontal;
        public float Vertical => _WalkData.Vertical;

        
        
        public WalkModule(WalkParameters parameters) : base() {
            this._Parameters = parameters;
        }

        public override void Start() {
            ContainerHolder.Container.BuildUp(this);
            _GroundedData = BB.Get<GroundedData>();
            _WallSlideData = BB.Get<WallSlideData>();
            _WalkData = BB.Get<WalkData>();
            _WalkData.Direction = 1;
        }

        public override void Initialize(Blackboard bb)
        {
            base.Initialize(bb);
            _characterAnimation = CommonData.MovementController
                .GetComponentInChildren<CharacterAnimationController>();
        }

        public override void FixedUpdate()
        {
            var xVelocity = CommonData.ObjRigidbody.velocity.x;
            var xLocalvelocity = xVelocity;
            var attachedRb = CommonData.MovementController.AttachedToRB;
            if (attachedRb != null)
                xLocalvelocity -= attachedRb.velocity.x;
            var airAcceleration = CommonData.MovementController.OverrideAirAcceleration
                ? CommonData.MovementController.OverridedAirAcceleration
                : _Parameters.AirAcceleration;
            var acceleration = _GroundedData.MainGrounded ? _Parameters.GroundAcceleration : airAcceleration;
            xLocalvelocity = Mathf.Lerp(xLocalvelocity, _TargetXVelocity, Time.fixedDeltaTime * acceleration);
            xVelocity = xLocalvelocity;
            if (attachedRb != null)
                xVelocity += attachedRb.velocity.x;
            CommonData.ObjRigidbody.velocity = new Vector2(xVelocity, CommonData.ObjRigidbody.velocity.y);
        }

        public override void Update() {
            SetDirection();
            _TargetXVelocity = 0f;
            
            if (_StopAnimatorStateNames != null && _StopAnimatorStateNames.Count > 0)
            {
                if (_StopAnimatorStateNames.Any(_ => _characterAnimation.CurrentAnimationState.IsName(_)))
                {
                    SetHorizontal(0);
                }
            }
            
            if (_WalkData.Horizontal > 0.15f) {
                _TargetXVelocity = _Parameters.Speed;
                ProcessRunSound(true);
            } else if (_WalkData.Horizontal < -0.15f) {
                _TargetXVelocity = -_Parameters.Speed;
                ProcessRunSound(true);
            } else {
                _WalkData.Horizontal = 0;
                ProcessRunSound(false);
            }
            // if (CommonData.WeaponController.MeleeAttacking) {
            //     _TargetXVelocity *= 0.8f;
            // }
        }

        private AudioEffect _RunSoundEffect;
        private void ProcessRunSound(bool moving) {
            if (string.IsNullOrEmpty(_Parameters.RunSoundEffectName) || !_GroundedData.Grounded || !moving) {
                StopIfHasEffect();
                return;
            }
            if (_RunSoundEffect == null)
                _RunSoundEffect = _AudioService.PlaySound3D(_Parameters.RunSoundEffectName, false, false, CommonData.MovementController.transform.position);
            else
                _RunSoundEffect.transform.position = CommonData.ObjTransform.position;
        }

        private void StopIfHasEffect() {
            if (_RunSoundEffect != null) {
                _RunSoundEffect.Stop(false);
                _RunSoundEffect = null;
            }
        }

        public void SetHorizontal(float hor) {
            _WalkData.Horizontal = hor;
            Mathf.Clamp(_WalkData.Horizontal, -1f, 1f);
        }

        public void SetVertical(float vert) {
            _WalkData.Vertical = vert;
            Mathf.Clamp(_WalkData.Vertical, -1f, 1f);
        }

        private void SetDirection() {
            if (_WalkData.Horizontal == 0)
                return;
            var newDir = _WalkData.Horizontal > 0 ? 1 : -1;
            CommonData.MovementController.ChangeDirection(newDir);
        }

        private List<string> _StopAnimatorStateNames;
        
        public void SetStopAnimatorStateNames(List<string> stateNames)
        {
            _StopAnimatorStateNames = stateNames;
        }
        
        // private int _MovementBlocks;
        // public void AddMovementBlock(MonoBehaviour behaviour, float blockTime)
        // {
        //     behaviour.StartCoroutine(AddMovementBlockRoutine(blockTime));
        // }
        //
        // private IEnumerator AddMovementBlockRoutine(float blockTime)
        // {
        //     _MovementBlocks++;
        //     yield return new WaitForSeconds(blockTime);
        //     _MovementBlocks--;
        // }
    }

    [Serializable]
    public class WalkParameters {
        public float Speed = 1f;
        public float GroundAcceleration = 1f;
        public float AirAcceleration = 1f;
        public Transform IkTransform;
        public string RunSoundEffectName;
    }
}