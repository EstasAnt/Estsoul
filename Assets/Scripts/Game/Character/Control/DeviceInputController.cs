using System;
using KlimLib.SignalBus;
using UI.Controll.DeviceControll;
using UnityDI;

namespace Character.Control
{
    public class DeviceInputController : InputController
    {
        [Dependency] private readonly SignalBus _SignalBus;

        protected override void Awake()
        {
            base.Awake();
            ContainerHolder.Container.BuildUp(this);
            _SignalBus.Subscribe<DeviceActionButtonPressSignal>(OnDeviceActionButtonPressSignal, this);
        }

        private void OnDeviceActionButtonPressSignal(DeviceActionButtonPressSignal signal)
        {
            switch (signal.DevicePlayerActionType)
            {
                case DevicePlayerActionType.Jump:
                    switch (signal.ButtonEventType)
                    {
                        case ButtonEventType.Press:
                            _PlayerController.Jump();
                            break;
                        case ButtonEventType.Hold:
                            _PlayerController.HoldJump();
                            break;
                        case ButtonEventType.Release:
                            _PlayerController.ReleaseJump();
                            break;
                    }
                    break;
                case DevicePlayerActionType.Action:
                    switch (signal.ButtonEventType)
                    {
                        case ButtonEventType.Press:
                        _PlayerController.Action();
                            break;
                    }

                    break;
                case DevicePlayerActionType.Roll:
                    switch (signal.ButtonEventType)
                    {
                        case ButtonEventType.Press:
                            _PlayerController.Roll();
                            break;
                    }
                    break;
                case DevicePlayerActionType.Attack:
                    switch (signal.ButtonEventType)
                    {
                        case ButtonEventType.Press:
                            _PlayerController.PressFire();
                            break;
                        case ButtonEventType.Hold:
                            _PlayerController.HoldFire();
                            break;
                        case ButtonEventType.Release:
                            _PlayerController.ReleaseFire();
                            break;
                    }
                    break;
            }
        }
        
        protected override void Move()
        {
            var axis = SimpleJoystick.Instance.Axis;
            _PlayerController.SetHorizontal(axis.x);
            _PlayerController.SetVertical(axis.y);
        }

        protected override void Jump()
        {

        }

        protected override void Roll()
        {

        }

        protected override void Attack()
        {

        }

        protected override void Action()
        {

        }

        protected override void Pause()
        {

        }

        private void OnDestroy()
        {
            _SignalBus.UnSubscribeFromAll(this);
        }
    }
}