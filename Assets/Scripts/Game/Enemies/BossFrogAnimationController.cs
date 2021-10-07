using System;
using Character.Health;
using Character.Shooting;
using UnityDI;
using UnityEngine;
using System.Collections;

namespace Game.Movement.Enemies
{
    public class BossFrogAnimationController : MonoBehaviour
    {
        private Animator _Animator;
        private WeaponController _WeaponController;
        private IDamageable _Damageable;
        
        private bool _Awake;

        private float _AwakeTimer;
        
        private void Awake() {
            ContainerHolder.Container.BuildUp(this);
            _Animator = GetComponent<Animator>();
            _WeaponController = GetComponentInParent<WeaponController>();
            _Damageable = GetComponent<IDamageable>();
            _Damageable.OnDamage += DamageableOnOnDamage;
        }

        private void Update()
        {
            if (_Animator == null) 
                return;
            _Animator.SetBool("Awake", _Awake);
        }

        private void DamageableOnOnDamage(IDamageable arg1, Damage arg2)
        {
            StopAllCoroutines();
            StartCoroutine(WakeUpRoutine());
        }

        private void OnDestroy()
        {
            _Damageable.OnDamage -= DamageableOnOnDamage;
        }

        private IEnumerator WakeUpRoutine()
        {
            _Awake = true;
            yield return new WaitForSeconds(1f);
            // _AwakeTimer = 0f;
            // while (_AwakeTimer < 3f)
            // {
            //     _AwakeTimer += Time.deltaTime;
            //     yield return null;
            // }
            _Awake = false;
        }
        
    }
}