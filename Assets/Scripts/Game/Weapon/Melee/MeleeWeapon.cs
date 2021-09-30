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

        [SerializeField]
        private Animator _Animator;
        
        public List<Attack> Attacks;

        public int AttacksInCombo
        {
            get
            {
                return _attacksInCombo;
            }
            set
            {
                if(_attacksInCombo != value)
                    CurrentAttackInComboChanged?.Invoke(_attacksInCombo);
                _attacksInCombo = value;
            }
        }

        private int _attacksInCombo;
        
        public bool CanReceiveInput { get; set; } = true;
        
        [SerializeField] private string _ShotCameraShakePresetName;

        private List<string> _AttacksAnimationNames = new List<string>();

        public event Action<int> CurrentAttackInComboChanged;
        
        protected override void Awake()
        {
            base.Awake();
            _InputProcessor = new AttackProcessor(this);
            if(_Animator == null)
                _Animator = GetComponentInParent<Animator>();
            _AttacksAnimationNames = Attacks.Select(_ => _.AnimationStateName).ToList();
        }

        private bool _LastFrameAttackAnimation;
        
        protected void LateUpdate()
        {
            var currentFrameIsAttackAnim = _Animator.OneOfAnimationsIsPlaying(_AttacksAnimationNames);
            if (_LastFrameAttackAnimation && !currentFrameIsAttackAnim)
            {
                AttacksInCombo = 0;
                Debug.LogError($"Reset Combo");
            }
            _LastFrameAttackAnimation = currentFrameIsAttackAnim;
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
            if (CanReceiveInput)
            {
                var attacksInCombo = AttacksInCombo;
                attacksInCombo++;
                if (attacksInCombo > Attacks.Count)
                    attacksInCombo = 1;
                AttacksInCombo = attacksInCombo;
                if(AttacksInCombo == 1)
                    ThrowAnimationTriggerEvent();
                // CanReceiveInput = false;
                Debug.LogError($"AttacksInCombo {AttacksInCombo}");
            }
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


        // protected override string GetAnimationTriggerName()
        // {
        //     return Attacks[AttacksInCombo].AnimationTriggerName;
        // }
    }
}