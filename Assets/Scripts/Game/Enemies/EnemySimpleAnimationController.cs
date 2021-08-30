using System;
using Character.Health;
using Character.Shooting;
using UnityDI;
using UnityEngine;

namespace Game.Movement.Enemies
{
    public class EnemySimpleAnimationController : MonoBehaviour {
        private MovementControllerBase _MovementController;
        private Animator Animator;

        private IDamageable _Damageable;
        
        private void Awake() {
            ContainerHolder.Container.BuildUp(this);
            Animator = GetComponent<Animator>();
            _MovementController = GetComponentInParent<MovementControllerBase>();
            _Damageable = GetComponentInParent<SimpleDamageable>();
        }

        private void Start()
        {
            if (_Damageable != null)
            {
                _Damageable.OnKill += DamageableOnOnKill;
            }
        }

        private void DamageableOnOnKill(IDamageable arg1, Damage arg2)
        {
            Animator.SetTrigger("Death");
        }

        private void Update()
        {
            if (_MovementController == null || Animator == null) 
                return;
            Animator.SetFloat("Horizontal", Mathf.Abs(_MovementController.Horizontal));
            Animator.SetFloat("Speed", Mathf.Abs(_MovementController.LocalVelocity.x));
        }

        private void OnDestroy()
        {
            if (_Damageable != null)
            {
                _Damageable.OnKill -= DamageableOnOnKill;
            }
        }
    }
}