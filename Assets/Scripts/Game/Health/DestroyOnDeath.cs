using Character.Health;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnDeath : MonoBehaviour {
    public IDamageable Damageable;

    private void Awake() {
        if(Damageable == null)
            Damageable = GetComponentInParent<HealthDamageable>();
        Damageable.OnKill += DestroyThisOnDeath;
    }

    private void OnValidate() {
        if (Damageable == null)
            Damageable = GetComponentInParent<HealthDamageable>();
    }

    private void DestroyThisOnDeath(IDamageable victim, Damage dmg) {
        Destroy(gameObject);
    }

    private void OnDestroy() {
        Damageable.OnKill -= DestroyThisOnDeath;
    }
}
