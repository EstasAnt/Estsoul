﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Character.Control;
using Character.Health;
using Character.Movement;
using Character.Shooting;
using Game.CameraTools;
using KlimLib.SignalBus;
using UnityEngine;
using UnityDI;
using Core.Services.Game;
using System;
using Core.Audio;
using Game.AI.CustomBehaviours.Behaviours;
using Game.Movement;
using Game.Weapons;
using KlimLib.ResourceLoader;
using Tools.VisualEffects;

public class CharacterUnit : MonoBehaviour, IDamageable, ICameraTarget, IWeaponHolder, IMobsTarget {
    [Dependency]
    private readonly SignalBus _SignalBus;
    [Dependency]
    private readonly AudioService _AudioService;
    [Dependency]
    private readonly IResourceLoaderService _ResourceLoader;

    public MovementController MovementController { get; private set; }

    public byte Id => OwnerId;
    MovementControllerBase IWeaponHolder.MovementController => MovementController;
    public WeaponController WeaponController { get; private set; }
    public IDamageable Damageable => this;

    public DamageBuffer DamageBuffer { get; private set; }

    public static List<CharacterUnit> Characters = new List<CharacterUnit>();

    public float Health { get; set; }
    public float MaxHealth { get; private set; }
    public float NormilizedHealth => Health / MaxHealth;

    public bool Dead { get; set; }
    
    public event Action<IDamageable, Damage> OnKill;
    public event Action<IDamageable, Damage> OnDamage;

    [SerializeField]
    private byte _OwnerId;
    public byte OwnerId { get; private set; }
    public string CharacterId;
    public List<string> HitAudioEffects;
    public List<string> DeathAudioEffects;

    private void Awake() {
        ContainerHolder.Container.BuildUp(this);
        MovementController = GetComponent<MovementController>();
        WeaponController = GetComponent<WeaponController>();
        DamageBuffer = GetComponent<DamageBuffer>();
        Collider = GetComponent<Collider2D>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Characters.Add(this);
        OwnerId = _OwnerId;
        Health = MaxHealth;
    }

    public Collider2D Collider { get; set; }
    public Transform Transform => transform;
    public Rigidbody2D Rigidbody2D { get; set; }

    public Vector3 Position => transform.position;
    public Vector3 Velocity => MovementController.Velocity;
    public float Direction => MovementController.Direction;

    byte? IDamageable.OwnerId => OwnerId;

    public void ApplyDamage(Damage damage) {
        _SignalBus.FireSignal(new ApplyDamageSignal(damage));
        OnDamage?.Invoke(this, damage);
        PlayeHitSound(HitAudioEffects);
    }

    private void PlayeHitSound(List<string> sounds) {
        if (sounds == null || sounds.Count == 0)
            return;
        var randIndex = UnityEngine.Random.Range(0, sounds.Count);
        _AudioService.PlaySound3D(sounds[randIndex], false, false, transform.position);
    }

    public void Initialize(byte ownerId, string characterId) {
        ContainerHolder.Container.BuildUp(this);
        OwnerId = ownerId;
        CharacterId = characterId;
        MaxHealth = 100f; //Todo: Config
        Health = MaxHealth;
        DamageBuffer?.Initialize(this, 3f);
    }

    private void OnDestroy() {
        _SignalBus?.FireSignal(new GameCameraTargetsChangeSignal(this, false));
        _SignalBus?.UnSubscribeFromAll(this);
        Characters.Remove(this);
    }

    public void Kill(Damage damage) {
        if (Dead)
            return;
        Dead = true;
        OnKill?.Invoke(this, damage);
        Debug.Log($"Player {OwnerId} character {CharacterId} dead.");
        Destroy(gameObject); //ToDo: something different
        _SignalBus?.FireSignal(new CharacterDeathSignal(damage));
        PlayeHitSound(DeathAudioEffects);
    }
}