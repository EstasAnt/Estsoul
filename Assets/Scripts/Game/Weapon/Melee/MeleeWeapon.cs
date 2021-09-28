using System;
using System.Collections.Generic;
using System.Linq;
using Character.Shooting;
using Com.LuisPedroFonseca.ProCamera2D;
using Game.Character.Shooting;
using Game.Movement;
using Game.Weapons;
using KlimLib.SignalBus;
using Spine.Unity;
using UnityDI;
using UnityEngine;

namespace Game.Character.Melee
{
    public class MeleeWeapon : Weapon
    {
        public override ItemType ItemType => ItemType.Weapon;
        public override WeaponInputProcessor InputProcessor => _InputProcessor;

        private WeaponInputProcessor _InputProcessor;

        public List<Attack> Attacks;

        public int ActiveAttackIndex { get; set; }
        
        [SerializeField] private string _ShotCameraShakePresetName;

        protected override void Awake()
        {
            base.Awake();
            _InputProcessor = new SingleShotProcessor(this);
            // GetComponentsInChildren(Attacks);
        }

        public override bool PickUp(IWeaponHolder owner)
        {
            var pickedUp = base.PickUp(owner);
            if (pickedUp)
            {
                owner.MovementController?.AddDontMoveAnimationStateNames(Attacks.Where(_ => _.StopWhileAttack)
                    .Select(_ => new DontMoveAnimationInfo(_.AnimationStateName, _.StopWhileAttackInAir)).ToList());
                
                owner.MovementController?.AddCantDirectAnimationStateNames(Attacks.Where(_ => _.CanDirectWhileAttack)
                    .Select(_ => _.AnimationStateName).ToList());
            }

            return pickedUp;
        }

        public override void PerformShot()
        {
            base.PerformShot();
            if (ProCamera2DShake.Instance != null && !string.IsNullOrEmpty(_ShotCameraShakePresetName))
                ProCamera2DShake.Instance.Shake(_ShotCameraShakePresetName);
            // Debug.LogError("Attack signal sent!");
        }

        public override void Hit(int attackIndex)
        {
            base.Hit(attackIndex);
            Attacks[attackIndex].Use();
        }

        public override void Dash(int attackIndex)
        {
            base.Dash(attackIndex);
            Attacks[attackIndex].Dash();
        }


        protected override string GetAnimationTriggerName()
        {
            return Attacks[ActiveAttackIndex].AnimationTriggerName;
        }
    }
}