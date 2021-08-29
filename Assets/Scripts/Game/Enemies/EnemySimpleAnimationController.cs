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

        private SimpleDamageable _simpleDamageable;
        
        private void Awake() {
            ContainerHolder.Container.BuildUp(this);
            Animator = GetComponent<Animator>();
            _MovementController = GetComponentInParent<MovementControllerBase>();
            _simpleDamageable = GetComponentInParent<SimpleDamageable>();
        }

        private void Start()
        {
            if (_simpleDamageable != null)
            {
                _simpleDamageable.OnKill += SimpleDamageableOnOnKill;
            }
        }

        private void SimpleDamageableOnOnKill(SimpleDamageable arg1, Damage arg2)
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
            if (_simpleDamageable != null)
            {
                _simpleDamageable.OnKill -= SimpleDamageableOnOnKill;
            }
        }
    }
}