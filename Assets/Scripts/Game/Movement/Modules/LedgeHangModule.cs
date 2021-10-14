using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Tools;
using Character.Control;
using Game.Movement;
using Game.Movement.Modules;
using KlimLib.SignalBus;
using UnityDI;
using UnityEngine;

namespace Character.Movement.Modules {
    public class LedgeHangModule : MovementModule
    {
        [Dependency] private readonly SignalBus _SignalBus;
        public bool LedgeHang => _WallSlideData.LedgeHanging;

        private LedgeHangParameters _Parameters;

        private WallSlideData _WallSlideData;
        private GroundedData _GroundedData;
        private JumpData _JumpData;
        private WalkData _WalkData;

        private float _StartGravityScale;

        public bool CanLedgeHang { get; set; }

        public event Action<string> AnimationTriggerEvent;
        
        public LedgeHangModule(LedgeHangParameters parameters) {
            _Parameters = parameters;
        }

        public override void Start()
        {
            _WalkData = BB.Get<WalkData>();
            _WallSlideData = BB.Get<WallSlideData>();
            _GroundedData = BB.Get<GroundedData>();
            _JumpData = BB.Get<JumpData>();
            _StartGravityScale = CommonData.ObjRigidbody.gravityScale;
            var ledgeHangAnimNames = new List<string>() {"LedgeHanging", "LedgeHangIdle", "PullUp"};
            var dontMoveList = ledgeHangAnimNames.Select(_ => new DontMoveAnimationInfo(_, true)).ToList();
            CommonData.MovementController.AddCantDirectAnimationStateNames(ledgeHangAnimNames);
            CommonData.MovementController.AddDontMoveAnimationStateNames(dontMoveList);
            _SignalBus.Subscribe<PlayerActionWasPressedSignal>(PlayerActionWasPressed, this);
        }

        public override void Update() {
            if(!CanLedgeHang)
                return;
            var timeSinceLastJump = Time.time - _JumpData.LastWallJumpTime;
            _WallSlideData.LedgeHanging = !_GroundedData.MainGrounded &&
                timeSinceLastJump > 0.2f &&
                _Parameters.UpSensor.IsTouching &&
            Time.timeSinceLevelLoad - _JumpAwayTime > 0.5f;
            if (_WallSlideData.LedgeHanging) {
                var firstTrigger = _Parameters.UpSensor.TouchedColliders.FirstOrDefault(_ => _.GetComponent(typeof(WallHangTrigger)));
                if (firstTrigger != null) {
                    var newLedgePos = Vector2.Lerp(_Parameters.LedgeHangPoint.position.ToVector2(),
                        firstTrigger.transform.position.ToVector2(), Time.fixedDeltaTime * _Parameters.LerpToLedgeSpeed);
                    var moveDelta = newLedgePos - _Parameters.LedgeHangPoint.position.ToVector2();
                    CommonData.ObjRigidbody.MovePosition(CommonData.ObjRigidbody.position + moveDelta);
                }
                CommonData.ObjRigidbody.gravityScale = 0;
                CommonData.ObjRigidbody.velocity = Vector2.zero;
                // SetDirection();
            }
            else {
                CommonData.ObjRigidbody.gravityScale = _StartGravityScale;
            }
        }

        private float _JumpAwayTime = float.NegativeInfinity;
        
        private void PlayerActionWasPressed(PlayerActionWasPressedSignal signal)
        {
            if (signal.PlayerAction == UniversalPlayerActions.Jump)
            {
                if (LedgeHang)
                {
                    if (_WalkData.VerticalAxis < 0)
                    {
                        _JumpAwayTime = Time.timeSinceLevelLoad;
                        return;
                    }
                    if (Mathf.Abs(_WalkData.HorizontalAxis) > 0 && Mathf.Sign(_WalkData.Direction) != _WalkData.HorizontalAxis)
                    {
                        if (CommonData.MovementController is MovementController controller)
                        {
                            _JumpAwayTime = Time.timeSinceLevelLoad;
                            controller.Jump(true);
                        }
                    }
                    else
                    {
                        AnimationTriggerEvent?.Invoke(_Parameters.PullUpAnimationTriggerName);
                    }
                }
            }
        }
    }

    [Serializable]
    public class LedgeHangParameters {
        public Sensor UpSensor;
        public Transform LedgeHangPoint;
        public string PullUpAnimationTriggerName;
        public float LerpToLedgeSpeed = 10f;
    }
}