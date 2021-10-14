using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Character.Movement;
using Character.Shooting;
using UnityEngine;
using Core.Services.Game;
using Game.Movement;
using InControl;
using KlimLib.SignalBus;
using UnityDI;
using UI;

namespace Character.Control {
    public class PlayerController : MonoBehaviour
    {
        [Dependency] private readonly SignalBus _signalBus;

        private WeaponController _WeaponController;
        private MovementController _MovementController;
        private IAimProvider _AimProvider;

        private Camera _Camera;
        private bool _IsJumping;
        private bool _WallJump;
        private bool _IsWallJumping;

        private void Awake() {
            _MovementController = GetComponent<MovementController>();
            _WeaponController = GetComponent<WeaponController>();
        }

        private void Start() {
            ContainerHolder.Container.BuildUp(this);
            _Camera = Camera.main;
            // _AimProvider = CurrentPlayerActions.Device == null
            //     ? (IAimProvider) new MouseAim(_Camera)
            //     : new JoystickAim(_WeaponController.Owner.MovementController.transform, _MovementController, CurrentPlayerActions);
        }

        public void SetHorizontal(float hor)
        {
            _MovementController.SetHorizontal(hor);
        }

        public void SetVertical(float vert)
        {
            _MovementController.SetVertical(vert);
        }

        public void Jump()
        {
            var fallDown = _MovementController.FallDownPlatform();
            if (!fallDown)
            {
                _IsJumping = _MovementController.Jump();
                if (!_IsJumping)
                {
                    _IsJumping = _MovementController.WallJump();
                    _WallJump = _IsJumping;
                }

                _MovementController.PressJump();
            }
            _signalBus.FireSignal(new PlayerActionWasPressedSignal(UniversalPlayerActions.Jump));
        }

        public void HoldJump()
        {
            _MovementController.ProcessHoldJump();
        }

        public void ReleaseJump()
        {
            _IsJumping = false;
            _WallJump = false;
            _MovementController.ReleaseJump();
        }
        
        public void Roll()
        {
            _MovementController.Roll();
        }

        public void PressFire()
        {
            _WeaponController.PressFire();
        }

        public void HoldFire()
        {
            _WeaponController.HoldFire();
        }

        public void ReleaseFire()
        {
            _WeaponController.ReleaseFire();
        }

        public void Action()
        {
            _signalBus.FireSignal(new PlayerActionWasPressedSignal(UniversalPlayerActions.Action));
        }

        public void Pause()
        {
            _signalBus.FireSignal(new MenuActionSignal());
        }

        // public void LateUpdate() {
        //     if(_AimProvider != null && _WeaponController.HasMainWeapon)
        //         _WeaponController.SetAimPosition(_AimProvider.AimPoint);
        // }
        
    }

    public enum UniversalPlayerActions
    {
        Jump,
        Action,
    }
}