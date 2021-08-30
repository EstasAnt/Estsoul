using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Character.Health;
using Items;
using UnityEngine;

namespace Character.Shooting {
    public class GrenadeProjectile : ThrowingProjectile {
        private IDamageable _Damageable;
        public float Timer;
        public ThrowingWeapon ThrowingWeapon { get; private set; }

        protected override void Awake() {
            base.Awake();
            _RB = GetComponent<Rigidbody2D>();
            _Damageable = GetComponent<HealthDamageable>();
            ThrowingWeapon = GetComponent<ThrowingWeapon>();
        }

        private void Start() {
            _Damageable.OnKill += DamageableOnOnKill;
        }

        private void DamageableOnOnKill(IDamageable arg1, Damage arg2)
        {
            PerformHit();
        }


        public override void Setup(ThrowingProjectileData data) {
            base.Setup(data);
            StartCoroutine(ExplosionRoutine());
        }
        
        
        
        private void PerformHit() {
            Setup(ThrowingWeapon.GetProjectileData());
            PerformHit(null, true);
        }

        private IEnumerator ExplosionRoutine() {
            yield return new WaitForSeconds(Timer);
            PerformHit();
        }

        protected override void OnDestroy() {
            base.OnDestroy();
            _Damageable.OnKill -= DamageableOnOnKill;
        }
    }
}
