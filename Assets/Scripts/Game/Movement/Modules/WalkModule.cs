using Core.Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Character.Melee;
using Tools.BehaviourTree;
using UnityDI;
using UnityEngine;

namespace Game.Movement.Modules {
    public class WalkModule : MovementModule {
        [Dependency]
        private readonly AudioService _AudioService;

        public float Direction => _WalkData.Direction;

        private GroundedData _GroundedData;
        private WallSlideData _WallSlideData;
        private WalkData _WalkData;

        private Animator _characterAnimator;
        
        private WalkParameters _Parameters;
        private float _TargetXVelocity = 0f;
        public float Horizontal => _WalkData.Horizontal;
        public float Vertical => _WalkData.Vertical;

        public MovementControllerBase MovementController => CommonData.MovementController;
        
        public WalkModule(WalkParameters parameters) : base() {
            this._Parameters = parameters;
        }

        private float _StartGravityScale;
        
        public override void Start() {
            ContainerHolder.Container.BuildUp(this);
            _GroundedData = BB.Get<GroundedData>();
            _WallSlideData = BB.Get<WallSlideData>();
            _WalkData = BB.Get<WalkData>();
            _WalkData.Direction = 1;
            _StartGravityScale = CommonData.ObjRigidbody.gravityScale;
        }

        public override void Initialize(Blackboard bb)
        {
            base.Initialize(bb);
            _characterAnimator = CommonData.MovementController
                .GetComponentInChildren<Animator>();
        }

        public override void FixedUpdate()
        {
            if(CommonData.MovementController.MovementBlock)
                return;
            if (_WallSlideData.LedgeHanging)
            {
                CommonData.ObjRigidbody.velocity = Vector2.zero;
                return;
            }
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

            if (_WallSlideData.LedgeHanging)
            {
                _WalkData.Horizontal = 0;
                _WalkData.Vertical = 0;
                //ToDo: Set direction to wall
                return;
            }
            if (!_characterAnimator.OneOfAnimationsIsPlaying(MovementController.CantChangeDirectionAnimatorStateNames))
            {
                Direct();
            }
            
            _TargetXVelocity = 0f;

            _WalkData.Horizontal = _WalkData.HorizontalAxis;
            _WalkData.Vertical = _WalkData.VerticalAxis;
            
            if (MovementController.DontMoveAnimatorStateNames != null && MovementController.DontMoveAnimatorStateNames.Count > 0)
            {
                var dontMoveAnimatonInfo = MovementController.DontMoveAnimatorStateNames.FirstOrDefault(_ =>
                    _characterAnimator.GetCurrentAnimatorStateInfo(0).IsName(_.AnimationName));
                if (dontMoveAnimatonInfo != null)
                {
                    if(_GroundedData.Grounded)
                        _WalkData.Horizontal = 0;
                    else
                    {
                        if (dontMoveAnimatonInfo.DontMoveInAir)
                            _WalkData.Horizontal = 0;
                    }
                }
            }

            if (_WalkData.Horizontal > 0.15f) {
                _TargetXVelocity = _Parameters.Speed * CommonData.MovementController.MovementSpeedBoostCoef;
                // ProcessRunSound(true);
            } else if (_WalkData.Horizontal < -0.15f) {
                _TargetXVelocity = -_Parameters.Speed * CommonData.MovementController.MovementSpeedBoostCoef;
                // ProcessRunSound(true);
            } else {
                _WalkData.Horizontal = 0;
                // ProcessRunSound(false);
            }
            // if (CommonData.WeaponController.MeleeAttacking) {
            //     _TargetXVelocity *= 0.8f;
            // }
        }

        private AudioEffect _RunSoundEffect;
        public void ProcessRunSound() {
            _AudioService.PlaySound3D(_Parameters.RunSoundEffectName, false, false, CommonData.ObjTransform.position);
            // if (string.IsNullOrEmpty(_Parameters.RunSoundEffectName) || !_GroundedData.Grounded || !moving) {
            //     StopIfHasEffect();
            //     return;
            // }
            // if (_RunSoundEffect == null)
            //     _RunSoundEffect = _AudioService.PlaySound3D(_Parameters.RunSoundEffectName, false, false, CommonData.MovementController.transform.position);
            // else
            //     _RunSoundEffect.transform.position = CommonData.ObjTransform.position;
            
        }

        private void StopIfHasEffect() {
            if (_RunSoundEffect != null) {
                _RunSoundEffect.Stop(false);
                _RunSoundEffect = null;
            }
        }

        public void SetHorizontalAxis(float hor) {
            _WalkData.HorizontalAxis = hor;
            Mathf.Clamp(_WalkData.HorizontalAxis, -1f, 1f);
        }

        public void SetVerticalAxis(float vert) {
            _WalkData.VerticalAxis = vert;
            Mathf.Clamp(_WalkData.HorizontalAxis, -1f, 1f);
        }

        public float Direct() {
            if (_WalkData.HorizontalAxis == 0)
                return 0;
            var newDir = _WalkData.HorizontalAxis > 0 ? 1 : -1;
            ChangeDirection(newDir);
            return _WalkData.HorizontalAxis;
        }
        
        public void ChangeDirection(int newDir) {
            if (_WalkData.Direction == newDir) //_WalkData.Direction == newDir
                return;
            _WalkData.Direction = newDir;
            var localScale = CommonData.MovementController.Root.localScale;
            var newLocalScale = new Vector3(newDir * Mathf.Abs(localScale.x), localScale.y, localScale.z);
            CommonData.MovementController.Root.localScale = newLocalScale;
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
        public string RunSoundEffectName;
    }
}