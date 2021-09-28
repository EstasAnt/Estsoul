using System;
using System.Collections.Generic;
using Character.Health;
using Character.Shooting;
using UnityDI;
using UnityEngine;

namespace Game.Movement.Enemies
{
    public class EnemySimpleAnimationController : MonoBehaviour {
        private MovementControllerBase _MovementController;
        private Animator Animator;
        private WeaponController _WeaponController;
        
        private IDamageable _Damageable;
        
        private void Awake() {
            ContainerHolder.Container.BuildUp(this);
            Animator = GetComponent<Animator>();
            _MovementController = GetComponentInParent<MovementControllerBase>();
            _Damageable = GetComponentInParent<SimpleDamageable>();
            _WeaponController = GetComponentInParent<WeaponController>();
        }

        private void Start()
        {
            if (_Damageable != null)
            {
                _Damageable.OnKill += DamageableOnOnKill;
                _Damageable.OnDamage += DamageableOnOnDamage;
            }
            _WeaponController.MainWeapon.AnimationTriggerEvent += MainWeaponOnAnimationTriggerEvent;


            var animStopList = new List<DontMoveAnimationInfo>();
            animStopList.Add(new DontMoveAnimationInfo("Hit", true));
            animStopList.Add(new DontMoveAnimationInfo("Death", true));
            _MovementController?.SetDontMoveAnimationStateNames(animStopList);
        }

        private void MainWeaponOnAnimationTriggerEvent(string obj)
        {
            Animator.SetTrigger(obj);
        }
        
        private void DamageableOnOnDamage(IDamageable arg1, Damage arg2)
        {
            Animator.SetTrigger("Hit");
        }

        private void DamageableOnOnKill(IDamageable arg1, Damage arg2)
        {
            Animator.SetTrigger("Death");
        }

        public void MainWeaponHit(int attackIndex)
        {
            _WeaponController.MainWeaponHit(attackIndex);
        }

        public void MainWeaponDash(int attackIndex)
        {
            _WeaponController.MainWeaponDash(attackIndex);
        }

        public void SetSeeTarget(bool val)
        {
            Animator.SetBool("SeeTarget", val);
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
                _Damageable.OnDamage -= DamageableOnOnDamage;
            }
            if(_WeaponController == null)
                return;
            if(_WeaponController.MainWeapon == null)
                return;
            _WeaponController.MainWeapon.AnimationTriggerEvent -= MainWeaponOnAnimationTriggerEvent;
        }
    }
}