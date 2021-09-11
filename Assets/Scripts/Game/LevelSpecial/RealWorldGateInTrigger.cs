namespace Game.LevelSpecial
{
    public class RealWorldGateInTrigger : TriggerSignalBroadcaster<CharacterUnit, RealWorldGateInSignal>
    {
        protected override bool UseTriggerExit => false;
        protected override RealWorldGateInSignal CreateSignal(CharacterUnit unit, bool inTrigger)
        {
            return new RealWorldGateInSignal()
            {
                characterUnit = unit,
                In = inTrigger,
            };
        }
    }
}