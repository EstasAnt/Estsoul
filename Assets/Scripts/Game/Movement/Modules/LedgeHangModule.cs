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
        public bool LedgeHang => _WallSlideData.LedgeHanging;

        private LedgeHangParameters _Parameters;

        private WallSlideData _WallSlideData;
        private GroundedData _GroundedData;
        private JumpData _JumpData;

        private float _StartGravityScale;

        public bool CanLedgeHang { get; set; }

        public LedgeHangModule(LedgeHangParameters parameters) {
            _Parameters = parameters;
        }

        public override void Start() {
            _WallSlideData = BB.Get<WallSlideData>();
            _GroundedData = BB.Get<GroundedData>();
            _JumpData = BB.Get<JumpData>();
            _StartGravityScale = CommonData.ObjRigidbody.gravityScale;
            var ledgeHangAnimNames = new List<string>() {"LedgeHanging", "LedgeHangIdle", "PullUp"};
            var dontMoveList = ledgeHangAnimNames.Select(_ => new DontMoveAnimationInfo(_, true)).ToList();
            CommonData.MovementController.AddCantDirectAnimationStateNames(ledgeHangAnimNames);
            CommonData.MovementController.AddDontMoveAnimationStateNames(dontMoveList);
        }

        public override void Update() {
            if(!CanLedgeHang)
                return;
            var timeSinceLastJump = Time.time - _JumpData.LastWallJumpTime;
            _WallSlideData.LedgeHanging = !_GroundedData.MainGrounded &&
                timeSinceLastJump > 0.2f &&
                // (_WallSlideData.LeftTouch || _WallSlideData.RightTouch) &&
                _Parameters.UpSensor.IsTouching;
            if (_WallSlideData.LedgeHanging) {
                var firstTrigger = _Parameters.UpSensor.TouchedColliders.FirstOrDefault(_ => _.GetComponent(typeof(WallHangTrigger)));
                if (firstTrigger != null) {
                    var posY = firstTrigger.transform.position.y;
                    var posX = firstTrigger.transform.position.x;
                    var deltaY = _Parameters.UpSensor.transform.position.y - CommonData.ObjTransform.position.y;
                    // CommonData.ObjRigidbody.MovePosition(new Vector2(posX, posY - deltaY));
                    // _LedgeHangCoroutine ??= CommonData.MovementController.StartCoroutine(LedgeHangRoutine(firstTrigger));
                    var ledgePointDelta = (_Parameters.LedgeHangPoint.position - CommonData.ObjRigidbody.transform.position).ToVector2();
                    var newLedgePos = Vector2.Lerp(_Parameters.LedgeHangPoint.position.ToVector2(),
                        firstTrigger.transform.position.ToVector2(), Time.fixedDeltaTime * 10f);
                    var moveDelta = newLedgePos - _Parameters.LedgeHangPoint.position.ToVector2();
                    CommonData.ObjRigidbody.MovePosition(CommonData.ObjRigidbody.position + moveDelta);
                    // yield return new WaitForFixedUpdate();
                    // delta = (_Parameters.LedgeHangPoint.position - firstTrigger.transform.position).ToVector2();
                }
                CommonData.ObjRigidbody.gravityScale = 0;
                CommonData.ObjRigidbody.velocity = Vector2.zero;
                SetDirection();
            }
            else {
                CommonData.ObjRigidbody.gravityScale = _StartGravityScale;
            }
        }

        private Coroutine _LedgeHangCoroutine;
        
        private IEnumerator LedgeHangRoutine(Collider2D targetCollider)
        {
            var delta = (_Parameters.LedgeHangPoint.position - targetCollider.transform.position).ToVector2();
            while (delta.sqrMagnitude > 0.01f)
            {
                var ledgePointDelta = (_Parameters.LedgeHangPoint.position - CommonData.ObjRigidbody.transform.position).ToVector2();
                var newLedgePos = Vector2.Lerp(_Parameters.LedgeHangPoint.position.ToVector2(),
                    targetCollider.transform.position.ToVector2(), Time.fixedDeltaTime * 2f);
                var moveDelta = newLedgePos - _Parameters.LedgeHangPoint.position.ToVector2();
                CommonData.ObjRigidbody.MovePosition(CommonData.ObjRigidbody.position + moveDelta);
                yield return new WaitForFixedUpdate();
                delta = (_Parameters.LedgeHangPoint.position - targetCollider.transform.position).ToVector2();
            }
        }

        private void SetDirection() {
            var newDir = 1;
            if (_WallSlideData.LeftTouch)
                newDir = -1;
            else if (_WallSlideData.RightTouch)
                newDir = 1;
            CommonData.MovementController.ChangeDirection(newDir);
        }
    }

    [Serializable]
    public class LedgeHangParameters {
        public Sensor UpSensor;
        public Transform LedgeHangPoint;
    }
}