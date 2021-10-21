using System;
using System.Linq;
using InControl;
using UnityEngine;

namespace Character.Control
{
    public class StationaryInputController : InputController
    {
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
        private PlayerActions _KeyboardActions;
        private PlayerActions _GamepadActions;
        
        
        protected override void Awake()
        {
            base.Awake();
            InitializeActions();
        }

        private void InitializeActions()
        {
            _KeyboardActions = PlayerActions.CreateWithKeyboardBindings();
            _GamepadActions = PlayerActions.CreateWithJoystickBindings();
        }

        protected override void Move()
        {
            var hor = CurrentPlayerActions.Move.Value.x;
            var vert = CurrentPlayerActions.Move.Value.y;
            _PlayerController.SetHorizontal(hor);
            _PlayerController.SetVertical(vert);
        }

        protected override void Jump()
        {
            if (CurrentPlayerActions.Jump.WasPressed)
            {
                _PlayerController.Jump();
            }
            if (CurrentPlayerActions.Jump)
            {
                _PlayerController.HoldJump();
            }
            if (CurrentPlayerActions.Jump.WasReleased)
            {
                _PlayerController.ReleaseJump();       
            }
        }
        
        protected override void Roll()
        {
            if (CurrentPlayerActions.Roll.WasPressed)
            {
                _PlayerController.Roll();
            }
        }
        
        protected override void Attack() {
            if (CurrentPlayerActions.Fire.WasPressed)
            {
                _PlayerController.PressFire();
            }
            if (CurrentPlayerActions.Fire) {
                _PlayerController.HoldFire();
            }
            if (CurrentPlayerActions.Fire.WasReleased) {
                _PlayerController.ReleaseFire();
            }
        }
        
        protected override void Action()
        {
            if (CurrentPlayerActions.Action.WasPressed)
            {
                _PlayerController.Action();
            }
        }

        protected override void Pause()
        {
            if (CurrentPlayerActions.Return.WasPressed)
            {
                _PlayerController.Pause();
            }
        }
    }
}