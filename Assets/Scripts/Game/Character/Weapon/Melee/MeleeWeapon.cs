using System.Collections.Generic;
using System.Linq;
using Character.Shooting;
using Com.LuisPedroFonseca.ProCamera2D;
using Game.Character.Shooting;
using KlimLib.SignalBus;
using UnityDI;
using UnityEngine;

namespace Game.Character.Melee
{
    public class MeleeWeapon : Weapon
    {
        [Dependency] private SignalBus _SignalBus;
        public override ItemType ItemType => ItemType.Weapon;
        public override WeaponInputProcessor InputProcessor => _InputProcessor;

        public List<Collider2D> AttackTriggers;
        
        private WeaponInputProcessor _InputProcessor;

        public List<Attack> Attacks;
        
        [SerializeField] private string _AnimationTrigger;
        public string AnimationTrigger => _AnimationTrigger;

        [SerializeField] private string _ShotCameraShakePresetName;

        protected override void Awake()
        {
            base.Awake();
            _InputProcessor = new SingleShotProcessor(this);
            GetComponentsInChildren(Attacks);
        }

        public override bool PickUp(CharacterUnit owner)
        {
            var pickedUp = base.PickUp(owner);
            if(pickedUp)
                owner.MovementController?.SetDontMoveAnimationStateNames(Attacks.Where(_=>_.StopWhileAttack).Select(_=>_.AnimationStateName).ToList());
            return pickedUp;
        }

        public override void PerformShot()
        {
            base.PerformShot();
            if (ProCamera2DShake.Instance != null && !string.IsNullOrEmpty(_ShotCameraShakePresetName))
                ProCamera2DShake.Instance.Shake(_ShotCameraShakePresetName);
            _SignalBus.FireSignal(new AttackAnimationSignal(_AnimationTrigger));
            Attacks.FirstOrDefault()?.Use();
            // Debug.LogError("Attack signal sent!");
        }
    }
}