using InControl;

namespace Character.Control
{
    public struct PlayerActionWasPressedSignal
    {
        public UniversalPlayerActions PlayerAction;
        public PlayerActionWasPressedSignal(UniversalPlayerActions playerAction)
        {
            PlayerAction = playerAction;
        }
    }
}