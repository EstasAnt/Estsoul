using System;
using System.Linq;
using InControl;
using UnityEngine;

namespace Character.Control
{
    public class StationaryInputController : MonoBehaviour
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
        
        private PlayerController _PlayerController;
        
        private void Awake()
        {
            _PlayerController = GetComponent<PlayerController>();
            InitializeActions();
        }

        private void InitializeActions()
        {
            _KeyboardActions = PlayerActions.CreateWithKeyboardBindings();
            _GamepadActions = PlayerActions.CreateWithJoystickBindings();
        }

        private void Update()
        {
            Move();
            Jump();
            Roll();
            Attack();
            Action();
            Pause();
#if FAST_SKIP_ENABLED
            FastSkip();
#endif
        }
        
        private void Move()
        {
            var hor = CurrentPlayerActions.Move.Value.x;
            var vert = CurrentPlayerActions.Move.Value.y;
            _PlayerController.SetHorizontal(hor);
            _PlayerController.SetVertical(vert);
        }

        private void Jump()
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
        
        private void Roll()
        {
            if (CurrentPlayerActions.Roll.WasPressed)
            {
                _PlayerController.Roll();
            }
        }
        
        private void Attack() {
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
        
        private void Action()
        {
            if (CurrentPlayerActions.Action.WasPressed)
            {
                _PlayerController.Action();
            }
        }

        private void Pause()
        {
            if (CurrentPlayerActions.Return.WasPressed)
            {
                _PlayerController.Pause();
            }
        }
#if FAST_SKIP_ENABLED
        private void FastSkip()
        {
            if (CurrentPlayerActions.FastSkip.WasPressed)
            {
                _PlayerController.FastSkip();
            }
        }
#endif
    }
}