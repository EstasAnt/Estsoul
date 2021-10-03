using System;
using System.Collections;
using System.Collections.Generic;
//using Character.CloseCombat;
using Character.Health;
using Core.Audio;
using Game.Character.Melee;
using Game.Weapons;
using Items;
using UnityDI;
using UnityEngine;

namespace Character.Shooting {
    public abstract class Weapon : MonoBehaviour {
        public abstract ItemType ItemType { get; }
        public virtual WeaponReactionType WeaponReaction => WeaponReactionType.Fire;

        public WeaponView WeaponView { get; protected set; }

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
        public List<string> HitAudioEffects;

        [SerializeField] private string _AnimationTrigger;
        public string AnimationTrigger => _AnimationTrigger;
        public event Action<string> AnimationTriggerEvent;
        
        [Dependency]
        protected readonly AudioService _AudioService;

        public abstract void PerformShot();

        protected virtual string GetAnimationTriggerName()
        {
            return AnimationTrigger;
        }

        protected virtual bool UseThrowForce => true;

        protected virtual void Awake()
        {
            WeaponView = GetComponent<WeaponView>();
        }

        protected virtual void Start() {
            ContainerHolder.Container.BuildUp(GetType(), this);
            WeaponsInfoContainer.AddWeapon(this);
        }

        public virtual bool PickUp(IWeaponHolder owner)
        {
            if (WeaponReaction == WeaponReactionType.Fire)
                owner?.WeaponController?.SubscribeWeaponOnEvents(this);
            else if (WeaponReaction == WeaponReactionType.Jump)
                owner?.MovementController?.SubscribeWeaponOnEvents(this);
            OnEquip();
            Owner = owner;
            return true;
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
        }

        protected virtual void ThrowAnimationTriggerEvent()
        {
            var animTrigger = GetAnimationTriggerName();
            if(!string.IsNullOrEmpty(animTrigger))
                AnimationTriggerEvent?.Invoke(GetAnimationTriggerName());
            // Debug.LogError($"Throw trigger event - {animTrigger}");
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

        public virtual void Hit(AttackInfoConfig info)
        {
            _AudioService.PlayRandomSound(HitAudioEffects, false, false, transform.position);
        }
        
        public virtual void Dash(AttackInfoConfig info)
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