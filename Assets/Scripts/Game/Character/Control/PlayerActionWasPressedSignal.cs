using InControl;

namespace Character.Control
{
    public struct PlayerActionWasPressedSignal
    {
        public PlayerAction PlayerAction;
        public PlayerActionWasPressedSignal(PlayerAction playerAction)
        {
            PlayerAction = playerAction;
        }
    }
}