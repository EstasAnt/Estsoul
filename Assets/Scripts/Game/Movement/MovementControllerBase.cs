using System;
using System.Collections.Generic;
using Character.Shooting;
using Tools.BehaviourTree;
using UnityEngine;

namespace Game.Movement
{
    public abstract class MovementControllerBase : MonoBehaviour
    {
        public Transform Root;
        
        public Collider2D GroundCollider;
        public Collider2D BodyCollider;

        protected List<MovementModule> _MovementModules;

        protected Blackboard _Blackboard;

        public Rigidbody2D Rigidbody { get; private set; }
        
        public virtual Vector2 Velocity => Rigidbody.velocity;
        public Vector2 LocalVelocity { get; private set; }
        
        public Rigidbody2D AttachedToRB { get; set; }

        public bool CanDetach { get; set; }
        
        public abstract float Horizontal { get; }

        public abstract float Direction { get; }
        
        public abstract bool IsGrounded { get; }
        
        public float OverridedAirAcceleration { get; set; }
        public bool OverrideAirAcceleration { get; set; }
        
        public event Action OnPressJump;
        public event Action OnHoldJump;
        public event Action OnReleaseJump;
        
        public bool MovementBlock { get; set; }
        
        public abstract bool CanMove { get;  }

        public float MovementSpeedBoostCoef { get; set; } = 1f;

        public IReadOnlyList<string> DontMoveAnimatorStateNames => _DontMoveAnimatorStateNames;

        public IReadOnlyList<string> CantChangeDirectionAnimatorStateNames => _CantChangeDirectionAnimatorStateNames;
        
        private List<string> _DontMoveAnimatorStateNames = new List<string>();
        
        private List<string> _CantChangeDirectionAnimatorStateNames = new List<string>();
        
        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            _MovementModules = CreateModules();
            SetupBlackboard();
            _MovementModules.ForEach(_ => _.Awake());
        }

        protected virtual void Start()
        {
            // _MovementModules = CreateModules();
            // SetupBlackboard();
            _MovementModules.ForEach(_ => _.Start());
        }

        protected abstract List<MovementModule> CreateModules();

        protected virtual void Update()
        {
            _MovementModules.ForEach(_ => _.Update());
            LocalVelocity = Velocity;
            if (!AttachedToRB)
            {
                AttachedToRB = null;
                CanDetach = true;
            }
            else
            {
                LocalVelocity -= AttachedToRB.velocity;
            }
        }

        protected virtual void LateUpdate()
        {
            _MovementModules.ForEach(_ => _.LateUpdate());
        }
        
        protected virtual void FixedUpdate() {
            _MovementModules.ForEach(_ => _.FixedUpdate());
        }
        
        protected virtual void OnCollisionEnter2D(Collision2D collision) {
            _MovementModules.ForEach(_ => _.OnCollisionEnter2D(collision));
            // if(CanDetach && AttachedToRB && Layers.Masks.LayerInMask(Layers.Masks.GroundAndPlatform, collision.gameObject.layer)) {
            //     AttachedToRB = null;
            // }
        }

        public void SetDontMoveAnimationStateNames(List<string> stateNames)
        {
            _DontMoveAnimatorStateNames = stateNames;
        }

        public void AddDontMoveAnimationStateNames(List<string> stateNames)
        {
            if(stateNames.IsNullOrEmpty())
                return;
            stateNames.ForEach(AddDontMoveAnimationStateName);
        }
        
        public void AddDontMoveAnimationStateName(string stateName)
        {
            if(!_DontMoveAnimatorStateNames.Contains(stateName))
                _DontMoveAnimatorStateNames.Add(stateName);
        }

        public void RemoveDontMoveAnimationStateName(string stateName)
        {
            if(_CantChangeDirectionAnimatorStateNames.Contains(stateName))
                _DontMoveAnimatorStateNames.Remove(stateName);
        }
        
        public void SetCantDirectAnimationStateNames(List<string> stateNames)
        {
            _CantChangeDirectionAnimatorStateNames = stateNames;
        }

        public void AddCantDirectAnimationStateNames(List<string> stateNames)
        {
            if(stateNames.IsNullOrEmpty())
                return;
            stateNames.ForEach(AddCantDirectAnimationStateName);
        }
        
        public void AddCantDirectAnimationStateName(string stateName)
        {
            if(!_CantChangeDirectionAnimatorStateNames.Contains(stateName))
                _CantChangeDirectionAnimatorStateNames.Add(stateName);
        }

        public void RemoveCantDirectAnimationStateName(string stateName)
        {
            if(_CantChangeDirectionAnimatorStateNames.Contains(stateName))
                _CantChangeDirectionAnimatorStateNames.Remove(stateName);
        }
        
        protected virtual void OnCollisionExit2D(Collision2D collision) {
            _MovementModules.ForEach(_ => _.OnCollisionExit2D(collision));
        }

        protected virtual void OnDestroy()
        {
            
        }

        protected abstract void SetupBlackboard();
        
        public abstract void SetHorizontal(float hor);
        
        public abstract void SetVertical(float hor);
        
        protected bool _JumpHold;
        
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

        public virtual void PlayMoveSound()
        {
            
        }
    }
}