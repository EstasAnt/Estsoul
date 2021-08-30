using System;
using System.Collections.Generic;
using Character.Health;
using Character.Movement.Modules;
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

        public float OverridedAirAcceleration { get; set; }
        public bool OverrideAirAcceleration { get; set; }
        
        
        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
        }

        protected virtual void Start()
        {
            _MovementModules = CreateModules();
            SetupBlackboard();
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
        
        protected virtual void OnCollisionExit2D(Collision2D collision) {
            _MovementModules.ForEach(_ => _.OnCollisionExit2D(collision));
        }

        protected virtual void OnDestroy()
        {
            
        }

        protected abstract void SetupBlackboard();
        
        public abstract void SetHorizontal(float hor);
        
        public abstract void SetVertical(float hor);
    }
}