using System;
using System.Collections;
using System.Collections.Generic;
//using Character.CloseCombat;
using Character.Health;
using Core.Audio;
using Game.Weapons;
using Items;
using UnityDI;
using UnityEngine;

namespace Character.Shooting {
    public abstract class Weapon : MonoBehaviour {
        public abstract ItemType ItemType { get; }
        public virtual WeaponReactionType WeaponReaction => WeaponReactionType.Fire;

        public WeaponView WeaponView { get; protected set; }
        // public PickableItem PickableItem { get; protected set; }

        public IWeaponHolder Owner { get; protected set; }
        
        public abstract WeaponInputProcessor InputProcessor { get; }

        [SerializeField]
        protected WeaponConfig _Stats;
        public WeaponPickupType PickupType;
        [SerializeField]
        private string _Id;
        public string Id => _Id;

        public WeaponConfig Stats => _Stats;

        public List<string> ShotSoundEffects;

        public event Action OnNoAmmo;

        [SerializeField] private string _AnimationTrigger;
        public string AnimationTrigger => _AnimationTrigger;
        public event Action<string> AnimationTriggerEvent;
        
        [Dependency]
        protected readonly AudioService _AudioService;

        public virtual void PerformShot() {
            if (InputProcessor.CurrentMagazine <= 0)
                OnNoAmmo?.Invoke();
            var animTrigger = GetAnimationTriggerName();
            if(!string.IsNullOrEmpty(animTrigger))
                AnimationTriggerEvent?.Invoke(GetAnimationTriggerName());
            PlayShotSound();
        }

        protected virtual string GetAnimationTriggerName()
        {
            return AnimationTrigger;
        }

        private void PlayShotSound() {
            if (ShotSoundEffects == null || ShotSoundEffects.Count == 0)
                return;
            _AudioService.PlaySound3D(ShotSoundEffects[UnityEngine.Random.Range(0, ShotSoundEffects.Count)], false, false, transform.position);
        }

        protected virtual bool UseThrowForce => true;

        protected virtual void Awake()
        {
            WeaponView = GetComponent<WeaponView>();
            // PickableItem = GetComponent<PickableItem>();
        }

        protected virtual void Start() {
            ContainerHolder.Container.BuildUp(GetType(), this);
            WeaponsInfoContainer.AddWeapon(this);
        }

        public virtual bool PickUp(IWeaponHolder owner)
        {
            // var pickedUp = false;
            // if (PickableItem != null)
            //     pickedUp = PickableItem.PickUp(owner);
            // else
            //     pickedUp = this is MeleeAttack;
            // if (pickedUp) {
            if (WeaponReaction == WeaponReactionType.Fire)
                owner?.WeaponController?.SubscribeWeaponOnEvents(this);
            else if (WeaponReaction == WeaponReactionType.Jump)
                owner?.MovementController?.SubscribeWeaponOnEvents(this);
            OnEquip();
            Owner = owner;
            return true;
            // }
            // return pickedUp;
        }

        public virtual void ThrowOut(IWeaponHolder owner) {
            switch (WeaponReaction) {
                case WeaponReactionType.Fire:
                    owner?.WeaponController?.UnSubscribeWeaponOnEvents(this);
                    break;
                case WeaponReactionType.Jump:
                    owner?.MovementController?.UnSubscribeWeaponOnEvents(this);
                    break;
            }
            Owner = null;
            OnLose();
            // PickableItem.ThrowOut(startVelocity, angularVel);
        }

        protected virtual Damage GetDamage() {
            return new Damage(Owner?.Id, null, _Stats.Damage);
        }

        protected virtual void Enable() {
            WeaponsInfoContainer.AddWeapon(this);
        }

        protected virtual void OnDisable() {
            WeaponsInfoContainer.RemoveWeapon(this);
            OnLose();
        }

        protected virtual void OnEquip() { }

        protected virtual void OnLose() { }

        public virtual void Hit()
        {
            
        }
        
        public virtual void Dash()
        {
            
        }
    }

    public enum ItemType {
        Weapon,
        Vehicle,
        MeleeAttack,
    }

    public enum WeaponReactionType {
        Fire,
        Jump,
    }

    public enum WeaponPickupType {
        ArmNear,
        Neck,
        None,
    }
}