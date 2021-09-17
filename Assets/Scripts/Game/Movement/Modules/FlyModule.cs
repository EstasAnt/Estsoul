using Core.Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Character.Melee;
using Tools.BehaviourTree;
using UnityDI;
using UnityEngine;

namespace Game.Movement.Modules
{
    public class FlyModule : MovementModule
    {
        [Dependency]
        private readonly AudioService _AudioService;

        public float Direction => _MoveData.Direction;

        private GroundedData _GroundedData;
        private WallSlideData _WallSlideData;
        private MoveData _MoveData;

        private Animator _characterAnimator;

        private FlyParameters _Parameters;
        private float _TargetXVelocity = 0f;
        public float Horizontal => _MoveData.Horizontal;
        public float Vertical => _MoveData.Vertical;



        public FlyModule(FlyParameters parameters) : base()
        {
            this._Parameters = parameters;
        }

        public override void Start()
        {
            ContainerHolder.Container.BuildUp(this);
            _GroundedData = BB.Get<GroundedData>();
            _WallSlideData = BB.Get<WallSlideData>();
            _MoveData = BB.Get<MoveData>();
            _MoveData.Direction = 1;
        }

        public override void Initialize(Blackboard bb)
        {
            base.Initialize(bb);
            _characterAnimator = CommonData.MovementController
                .GetComponentInChildren<Animator>();
        }

        public override void FixedUpdate()
        {
            if (CommonData.MovementController.MovementBlock)
                return;
            Vector2 Velocity= Vector2.Lerp(CommonData.ObjRigidbody.velocity, new Vector2(_MoveData.Horizontal, _MoveData.Vertical).normalized * _Parameters.Speed, Time.fixedDeltaTime * _Parameters.Acceleration);
            CommonData.ObjRigidbody.velocity = Velocity;
            Debug.LogError($"Vertical - {_MoveData.Vertical}, LocalVelocity - {CommonData.ObjRigidbody.velocity}, {Velocity}");
        }

        public override void Update()
        {/*
            SetDirection();
            _TargetXVelocity = 0f;

            if (_StopAnimatorStateNames != null && _StopAnimatorStateNames.Count > 0)
            {
                if (_StopAnimatorStateNames.Any(_ => _characterAnimator.GetCurrentAnimatorStateInfo(0).IsName(_)))
                {
                    SetHorizontal(0);
                }
            }

            if (_MoveData.Horizontal > 0.15f)
            {
                _TargetXVelocity = _Parameters.Speed * CommonData.MovementController.MovementSpeedBoostCoef;
                ProcessRunSound(true);
            }
            else if (_MoveData.Horizontal < -0.15f)
            {
                _TargetXVelocity = -_Parameters.Speed * CommonData.MovementController.MovementSpeedBoostCoef;
                ProcessRunSound(true);
            }
            else
            {
                _MoveData.Horizontal = 0;
                ProcessRunSound(false);
            }
            // if (CommonData.WeaponController.MeleeAttacking) {
            //     _TargetXVelocity *= 0.8f;
            // }*/
        }

        private AudioEffect _RunSoundEffect;
        private void ProcessRunSound(bool moving)
        {
            /*if (string.IsNullOrEmpty(_Parameters.RunSoundEffectName) || !_GroundedData.Grounded || !moving)
            {
                StopIfHasEffect();
                return;
            }
            if (_RunSoundEffect == null)
                _RunSoundEffect = _AudioService.PlaySound3D(_Parameters.RunSoundEffectName, false, false, CommonData.MovementController.transform.position);
            else
                _RunSoundEffect.transform.position = CommonData.ObjTransform.position;*/
        }

        private void StopIfHasEffect()
        {
            if (_RunSoundEffect != null)
            {
                _RunSoundEffect.Stop(false);
                _RunSoundEffect = null;
            }
        }

        public void SetHorizontal(float hor)
        {
            _MoveData.Horizontal = hor;
            Mathf.Clamp(_MoveData.Horizontal, -1f, 1f);
        }

        public void SetVertical(float vert)
        {
            _MoveData.Vertical = vert;
            Mathf.Clamp(_MoveData.Vertical, -1f, 1f);
        }

        private void SetDirection()
        {
            if (_MoveData.Horizontal == 0)
                return;
            var newDir = _MoveData.Horizontal > 0 ? 1 : -1;
            ChangeDirection(newDir);
        }

        private List<string> _StopAnimatorStateNames;

        public void SetStopAnimatorStateNames(List<string> stateNames)
        {
            _StopAnimatorStateNames = stateNames;
        }

        public void ChangeDirection(int newDir)
        {
            if (_MoveData.Direction == newDir) //_WalkData.Direction == newDir
                return;
            _MoveData.Direction = newDir;
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
    public class FlyParameters
    {
        public float Speed = 1f;
        public float Acceleration = 1f;
        public string RunSoundEffectName;
    }
}
