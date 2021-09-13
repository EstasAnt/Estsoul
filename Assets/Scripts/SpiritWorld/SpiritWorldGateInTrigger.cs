using Game.LevelSpecial;

namespace SceneManagement.SpiritWorld
{
    public class SpiritWorldGateInTrigger : TriggerSignalBroadcaster<PlayerController , SpiritWorldGateInSignal> {
        protected override SpiritWorldGateInSignal CreateSignal(PlayerController unit, bool enter)
        {
            return new SpiritWorldGateInSignal(enter);
        }
    }
}