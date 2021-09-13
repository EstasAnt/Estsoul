using Core.Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using Tools.VisualEffects;
using UnityDI;
using UnityEngine;

namespace Game.Movement.Modules {
    public class JumpModule : MovementModule {

        [Dependency]
        private readonly AudioService _AudioService;

        private JumpParameters _Parameters;

        private WallSlideData _WallSlideData;
        private GroundedData _GroundedData;
        private JumpData _JumpData;
        private WalkData _WalkData;
        
        private Coroutine _WallJumpWaitingRoutine;

        private bool _UsedAirJump = false;
        
        public JumpModule(JumpParameters parameters) {
            _Parameters = parameters;
        }

        public bool DoubleJump  => _UsedAirJump;

        public override void Start() {
            ContainerHolder.Container.BuildUp(this);
            _WallSlideData = BB.Get<WallSlideData>();
            _GroundedData = BB.Get<GroundedData>();
            _JumpData = BB.Get<JumpData>();
            _WalkData = BB.Get<WalkData>();
        }

        public override void LateUpdate() {
            _JumpData.Jump = false;
        }

        public override void Update()
        {
            if (_GroundedData.MainGrounded)
                _UsedAirJump = false;
        }

        private bool _GroundJumping = false;
        
        public bool Jump(MonoBehaviour behaviour) {
            if (!_GroundedData.Grounded || !(_GroundedData.TimeSinceMainGrounded < 0.3f)) 
                return false;
            behaviour.StopCoroutine(GroundJumpRoutine());
            behaviour.StartCoroutine(GroundJumpRoutine());
            SpawnJumpEffects();
            PlayAudioEffect();

            return true;
        }

        public bool AirJump(MonoBehaviour behaviour)
        {
            if (!_Parameters.AllowAirJump || _GroundedData.Grounded || _UsedAirJump) 
                return false;
            _UsedAirJump = true;
            behaviour.StopCoroutine(AirJumpRoutine());
            behaviour.StartCoroutine(AirJumpRoutine());
            return true;
        }

        private void SpawnJumpEffects() {
            if (_Parameters.JumpEffectTransformPoints.IsNullOrEmpty() || _Parameters.JumpEffectNames.IsNullOrEmpty())
                return;
            foreach (var point in _Parameters.JumpEffectTransformPoints) {
                var randIndex = UnityEngine.Random.Range(0, _Parameters.JumpEffectNames.Count);
                var effect = VisualEffect.GetEffect<ParticleEffect>(_Parameters.JumpEffectNames[randIndex]);
                effect.transform.position = point.transform.position;
                effect.transform.rotation = Quaternion.identity;
                effect.transform.localScale = new Vector3(Mathf.Abs(effect.transform.localScale.x) * _WalkData.Direction, effect.transform.localScale.y, effect.transform.localScale.z);
                effect.Play();
            }
        }

        private void PlayAudioEffect() {
            if (_Parameters.JumpAudioEffectNames.IsNullOrEmpty())
                return;
            var randIndex = UnityEngine.Random.Range(0, _Parameters.JumpAudioEffectNames.Count);
            _AudioService.PlaySound3D(_Parameters.JumpAudioEffectNames[randIndex], false, false, CommonData.ObjTransform.position);
        }

        private IEnumerator JumpRoutine(float jumpVelocity, int fixedUpdatesCount) {
            for (var i = 0; i < fixedUpdatesCount; i++)
            {
                CommonData.ObjRigidbody.velocity = new Vector2(CommonData.ObjRigidbody.velocity.x, jumpVelocity);
                // _JumpData.LastJumpTime = Time.time;
                i++;
                yield return new WaitForFixedUpdate();
            }
        }

        private IEnumerator GroundJumpRoutine()
        {
            _GroundJumping = true;
            yield return JumpRoutine(_Parameters.GroundJumpSpeed, _Parameters.GroundJumpFixedUpdates);
            _GroundJumping = false;
        }
        
        private IEnumerator AirJumpRoutine()
        {
            yield return new WaitUntil(() => !_GroundJumping);
            yield return new WaitForFixedUpdate();
            yield return JumpRoutine(_Parameters.AirJumpSpeed, _Parameters.AirJumpFixedUpdates);
        }
        
        private IEnumerator IWallJumpWaiting(MonoBehaviour behaviour) {
            float timeBeforeJump = 0.7f;

            for (int c = Mathf.RoundToInt(timeBeforeJump / Time.fixedDeltaTime); c > 0; c--) {
                yield return new WaitForFixedUpdate();
                if (WallJump(behaviour))
                    break;
            }

            _WallJumpWaitingRoutine = null;
        }

        public bool WallJump(MonoBehaviour behaviour)
        {
            if (!_Parameters.AllowWallJump)
                return false;
            if (_WallSlideData.RightTouch) {
                var vector = new Vector2(-1, 0.9f).normalized;
                CommonData.ObjRigidbody.velocity = vector * _Parameters.WallJumpSpeed;
                _JumpData.LastWallJumpTime = Time.time;
                PlayAudioEffect();
                return true;
            }
            if (_WallSlideData.LeftTouch) {
                var vector = new Vector2(1, 0.9f).normalized;
                CommonData.ObjRigidbody.velocity = vector * _Parameters.WallJumpSpeed;
                _JumpData.LastWallJumpTime = Time.time;
                PlayAudioEffect();
                return true;
            }

            if (_WallJumpWaitingRoutine == null)
                _WallJumpWaitingRoutine = behaviour.StartCoroutine(IWallJumpWaiting(behaviour));

            return false;
        }
    }

    [Serializable]
    public class JumpParameters {
        public float GroundJumpSpeed;
        public int GroundJumpFixedUpdates;
        public bool AllowWallJump;
        public float WallJumpSpeed;
        public bool AllowAirJump;
        public float AirJumpSpeed;
        public int AirJumpFixedUpdates;
        public List<Transform> JumpEffectTransformPoints;
        public List<string> JumpEffectNames;
        public List<string> JumpAudioEffectNames;
    }
}