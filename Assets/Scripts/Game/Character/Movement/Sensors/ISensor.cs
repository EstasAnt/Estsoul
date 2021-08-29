namespace Game.Movement.Modules
{
    public interface ISensor
    {
        bool IsTouching { get; }
        float Distanse { get; }
    }
}