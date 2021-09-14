using Character.Health;
using KlimLib.SignalBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityDI;
using UnityEngine;

namespace Character.Health {
    public class HealthDamageable : MonoBehaviour, IDamageable {
        [Dependency]
        private readonly SignalBus _SignalBus;

        [SerializeField]
        private float _MaxHealth;
        
        [SerializeField]
        public int _TeamIndex = 2;
        public byte? OwnerId => null;
        public float MaxHealth => _MaxHealth;
        public float Health { get; set; }
        public float NormilizedHealth => Health / MaxHealth;
        public float TeamIndex { get; }

        public bool DestroyOnDeath = true;

        public bool Dead { get; set; }
        public Collider2D Collider { get; set; }

        public event Action<IDamageable, Damage> OnKill;
        public event Action<IDamageable, Damage> OnDamage;
        
        private void Awake() {
            Collider = GetComponentInChildren<Collider2D>();
            Health = MaxHealth;
        }

        private void Start() {
            ContainerHolder.Container.BuildUp(this);
        }

        public void ApplyDamage(Damage damage) {
            OnDamage?.Invoke(this, damage);
            _SignalBus.FireSignal(new ApplyDamageSignal(damage));
        }

        public void Kill(Damage damage) {
            OnKill?.Invoke(this, damage);
            if(DestroyOnDeath)
                Destroy(gameObject);
        }
    }
}
