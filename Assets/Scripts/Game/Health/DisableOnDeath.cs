using Character.Health;
using UnityEngine;

public class DisableOnDeath : MonoBehaviour {
    private IDamageable Damageable;

    private void Awake() {
        Damageable = GetComponentInParent<HealthDamageable>();
        Damageable.OnKill += DisableThisOnDeath;
    }

    private void DisableThisOnDeath(IDamageable victim, Damage dmg) {
        gameObject.SetActive(false);
    }

    private void OnDestroy() {
        Damageable.OnKill -= DisableThisOnDeath;
    }
}