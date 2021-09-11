using Game.LevelSpecial;

namespace SceneManagement.SpiritWorld
{
    public class SpiritWorldGateInTrigger : TriggerSignalBroadcaster<PlayerController , SpiritWorldGateInSignal>
    {
        protected override bool UseTriggerExit => false;

        protected override SpiritWorldGateInSignal CreateSignal(PlayerController unit, bool inTrigger)
        {
            return new SpiritWorldGateInSignal(inTrigger);
        }
    }
}