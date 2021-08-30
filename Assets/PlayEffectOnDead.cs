using Core.Audio;
using System.Collections;
using System.Collections.Generic;
using Character.Health;
using Tools.VisualEffects;
using UnityDI;
using UnityEngine;

public class PlayEffectOnDead : MonoBehaviour {
    [Dependency]
    private readonly AudioService _AudioService;

    public string EffectOnKillName;

    private IDamageable _Damageable;

    private void Awake() {
        _Damageable = GetComponent<IDamageable>();
        _Damageable.OnKill += this.DamageableOnKill;
    }

    private void DamageableOnKill(IDamageable dmgbl, Damage dmg) {
        ContainerHolder.Container.BuildUp(this);
        var effect = VisualEffect.GetEffect<VisualEffect>(EffectOnKillName);
        effect.transform.position = transform.position;
        effect.transform.rotation = transform.rotation;
        effect.Play();
    }

    private void OnDestroy() {
        if (_Damageable != null) {
            _Damageable.OnKill -= this.DamageableOnKill;
        }
    }
}
