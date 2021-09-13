namespace Game.LevelSpecial
{
    public class RealWorldGateInTrigger : TriggerSignalBroadcaster<CharacterUnit, RealWorldGateInSignal>
    {
        protected override RealWorldGateInSignal CreateSignal(CharacterUnit unit, bool enter)
        {
            return new RealWorldGateInSignal()
            {
                characterUnit = unit,
                In = enter,
            };
        }
    }
}