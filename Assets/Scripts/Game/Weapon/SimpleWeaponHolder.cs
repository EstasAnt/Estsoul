using System.Collections;
using System.Collections.Generic;
using Character.Health;
using Character.Shooting;
using Game.Movement;
using Game.Weapons;
using UnityEngine;

public class SimpleWeaponHolder : MonoBehaviour, IWeaponHolder
{
    [SerializeField]
    private byte _Id;
    
    public MovementControllerBase MovementController { get; private set; }
    public byte Id => _Id;
    public WeaponController WeaponController { get; private set; }
    public IDamageable Damageable { get; private set; }

    private void Awake()
    {
        MovementController = GetComponent<MovementControllerBase>();
        WeaponController = GetComponent<WeaponController>();
        Damageable = GetComponent<IDamageable>();
    }
}
