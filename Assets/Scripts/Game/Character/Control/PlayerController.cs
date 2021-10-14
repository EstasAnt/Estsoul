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
        
        public int Id;
        public PlayerActions CurrentPlayerActions
        {
            get
            {
                if (_GamepadActions == null)
                    return _KeyboardActions;

                if (_GamepadActions.Device == null)
                    _GamepadActions.Device =
                        InputManager.Devices.FirstOrDefault(_ => _.DeviceClass == InputDeviceClass.Controller); //ToDo: Remove from here
                if (_KeyboardActions == null)
                    return _GamepadActions;
                if (_GamepadActions.Device == null)
                    return _KeyboardActions;
                if (_KeyboardActions.Device == null)
                    return _GamepadActions;
                return _KeyboardActions.Device.LastInputTick > _GamepadActions.Device.LastInputTick
                    ? _KeyboardActions
                    : _GamepadActions;
            }
        }
        //public const float PressTime2HighJump = 0.12f;
        private WeaponController _WeaponController;
        private MovementController _MovementController;
        private IAimProvider _AimProvider;

        private Camera _Camera;
        private bool _IsJumping;
        private bool _WallJump;
        private bool _IsWallJumping;

        private PlayerActions _KeyboardActions;
        private PlayerActions _GamepadActions;
        
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


        public void InitializeActions(PlayerActions keyboard, PlayerActions gamepad)
        {
            _KeyboardActions = keyboard;
            _GamepadActions = gamepad;
        }
        
        public void Update() {
            Move();
            Jump();
            Roll();
            Attack();
            Action();
            Pause();
        }
        

        private void Move() {
            var hor = CurrentPlayerActions.Move.Value.x;
            var vert = CurrentPlayerActions.Move.Value.y;
            _MovementController.SetHorizontal(hor);
            _MovementController.SetVertical(vert);
        }

        private void Jump() {
            if (CurrentPlayerActions.Jump.WasPressed) {
                var fallDown = _MovementController.FallDownPlatform();
                if (!fallDown) {
                    _IsJumping = _MovementController.Jump();
                    if (!_IsJumping) {
                        _IsJumping = _MovementController.WallJump();
                        _WallJump = _IsJumping;
                    }
                    _MovementController.PressJump();
                }
                _signalBus.FireSignal(new PlayerActionWasPressedSignal(CurrentPlayerActions.Jump));
            }

            if (CurrentPlayerActions.Jump) {
                _MovementController.ProcessHoldJump();
            }

            if (CurrentPlayerActions.Jump.WasReleased) {
                _IsJumping = false;
                _WallJump = false;
                _MovementController.ReleaseJump();
            }
        }

        private void Roll()
        {
            if (CurrentPlayerActions.Roll.WasPressed)
            {
                _MovementController.Roll();
            }
        }
        
        private void Attack() {
            if (CurrentPlayerActions.Fire.WasPressed)
            {
                _WeaponController.PressFire();
            }
            if (CurrentPlayerActions.Fire) {
                _WeaponController.HoldFire();
            }
            if (CurrentPlayerActions.Fire.WasReleased) {
                _WeaponController.ReleaseFire();
            }
        }

        private void Action()
        {
            if (CurrentPlayerActions.Action.WasPressed)
            {
                _signalBus.FireSignal(new PlayerActionWasPressedSignal(CurrentPlayerActions.Action));
            }
        }

        private void Pause()
        {
            if (CurrentPlayerActions.Return.WasPressed)
            {
                _signalBus.FireSignal(new MenuActionSignal());
            }
        }

        public void LateUpdate() {
            if(_AimProvider != null && _WeaponController.HasMainWeapon)
                _WeaponController.SetAimPosition(_AimProvider.AimPoint);
        }

        private void OnDrawGizmosSelected() {
            if (!Application.isPlaying)
                return;
            // Gizmos.color = _AimProvider is MouseAim ? Color.red : Color.yellow;
            // Gizmos.DrawWireSphere(_AimProvider.AimPoint, 1f);
            // Gizmos.DrawLine(_AimProvider.AimPoint, _WeaponController.NearArmShoulder.position);
        }
    }
}