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
        
        public List<AttackGroup> AttackGroups;

        public int SelectedAttackGroupIndex { get; private set; }

        public AttackGroup SelectedAttackGroup => AttackGroups[SelectedAttackGroupIndex];
        
        [SerializeField] 
        private bool _UseTriggerNameFromAttacks = true;
        public int AttacksInCombo
        {
            get
            {
                return _attacksInCombo;
            }
            protected set
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
            _AttacksAnimationNames = AttackGroups.SelectMany(_=> _.Attacks).Select(_ => _.AnimationStateName).ToList();
            _Animator = GetComponentInParent<WeaponController>().GetComponentInChildren<Animator>();
        }

        private bool _LastFrameAttackAnimation;
        
        protected void LateUpdate()
        {
            var currentFrameIsAttackAnim = _Animator.OneOfAnimationsIsPlaying(_AttacksAnimationNames);
            if (_LastFrameAttackAnimation && !currentFrameIsAttackAnim)
            {
                CanReceiveInput = true;
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
                owner.MovementController?.AddDontMoveAnimationStateNames(AttackGroups.SelectMany(_=>_.Attacks).Where(_ => _.StopWhileAttack)
                    .Select(_ => new DontMoveAnimationInfo(_.AnimationStateName, _.StopWhileAttackInAir)).ToList());
                
                owner.MovementController?.AddCantDirectAnimationStateNames(AttackGroups.SelectMany(_=>_.Attacks).Where(_ => _.CanDirectWhileAttack)
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
                if (attacksInCombo > SelectedAttackGroup.Attacks.Count)
                    attacksInCombo = 1;
                AttacksInCombo = attacksInCombo;
                if(AttacksInCombo == 1)
                    ThrowAnimationTriggerEvent();
                CanReceiveInput = false;
                Debug.LogError($"AttacksInCombo {AttacksInCombo}");
            }
        }

        public override void Hit(AttackInfoConfig info)
        {
            base.Hit(info);
            var currentAttack = AttackGroups[info.GroupNum].Attacks[info.AttackNum];
            currentAttack.Use();
        }

        public override void Dash(AttackInfoConfig info)
        {
            base.Dash(info);
            var currentAttack = AttackGroups[info.GroupNum].Attacks[info.AttackNum];
            currentAttack.Dash();
        }

        public void SelectAttackGroup(int index)
        {
            if(index >= AttackGroups.Count || index < 0)
                return;
            SelectedAttackGroupIndex = index;
        }
        
        protected override string GetAnimationTriggerName()
        {
            return _UseTriggerNameFromAttacks ? SelectedAttackGroup.AnimationTriggerName : base.GetAnimationTriggerName();
        }
    }
}