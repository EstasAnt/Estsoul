namespace UI.Controll.DeviceControll
{
    public struct DeviceActionButtonPressSignal
    {
        public DevicePlayerActionType DevicePlayerActionType;
        public ButtonEventType ButtonEventType;

    }

    public enum DevicePlayerActionType
    {
        Jump,
        Action,
        Roll,
        Attack,
        JumpBackSW,
        JumpForwardSW,
    }

    public enum ButtonEventType
    {
        Press,
        Hold,
        Release,
    }
}