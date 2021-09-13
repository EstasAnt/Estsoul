using Character.Health;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentsActionsOnDeath : MonoBehaviour {

    public List<ComponentAction> ComponentActions;
    private IDamageable Damageable;

    private void Awake() {
        Damageable = GetComponentInParent<IDamageable>();
        Damageable.OnKill += ComponentActionsOnDeath;
    }

    private void ComponentActionsOnDeath(IDamageable victim, Damage dmg) {
        foreach(var componentAction in ComponentActions) {
            componentAction.Component.enabled = componentAction.Enable;
        }
    }

    private void OnDestroy() {
        Damageable.OnKill -= ComponentActionsOnDeath;
    }
}

[Serializable]
public class ComponentAction {
    public Behaviour Component;
    public bool Enable = true;
}
