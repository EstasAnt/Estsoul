using System;
using System.Collections;
using System.Collections.Generic;
using Character.Health;
using KlimLib.SignalBus;
using UnityDI;
using UnityEngine;

public class SimpleDamageable : MonoBehaviour, IDamageable {
    [Dependency]
    private readonly HealthService _HealthService;
    [Dependency]
    private readonly SignalBus _SignalBus;

    public float StartHealth;
    public bool UseHealth = false;

    public float DestroyAfterKillTime;

    public bool DisablePhysicsAfterDeath = true;
    
    public Collider2D Collider { get; set; }

    public float Health { get; set; }

    public float NormilizedHealth => Health / MaxHealth;

    public byte? OwnerId => null;

    public float MaxHealth { get; set; }

    public bool Dead { get; set; }
    public event Action<IDamageable, Damage> OnDamage;
    public event Action<IDamageable, Damage> OnKill;

    public void ApplyDamage(Damage damage) {
        if(UseHealth)
            _SignalBus.FireSignal(new ApplyDamageSignal(damage));
        OnDamage?.Invoke(this, damage);
    }

    public void Kill(Damage damage) {
        Dead = true;
        if (DisablePhysicsAfterDeath)
        {
            var rb = GetComponent<Rigidbody2D>();
            Destroy(rb);
            var collliders = GetComponentsInChildren<Collider2D>();
            collliders.ForEach(_ => Destroy(_));
        }
        OnKill?.Invoke(this, damage);
        StartCoroutine(DestroyRoutine());
    }

    private IEnumerator DestroyRoutine()
    {
        yield return new WaitForSeconds(DestroyAfterKillTime);
        Destroy(gameObject);
    }
    
    private void Awake() {
        Collider = gameObject.GetComponentInChildren<Collider2D>();
    }

    private void Start()
    {
        ContainerHolder.Container.BuildUp(this);
        if (UseHealth) {
            MaxHealth = StartHealth;
            Health = MaxHealth;
        }
    }
}
