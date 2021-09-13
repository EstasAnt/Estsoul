namespace Game.Character.Shooting
{
    public struct AttackAnimationSignal
    {
        public string AnimationTriggerName;
        
        public AttackAnimationSignal(string animationTriggerName)
        {
            AnimationTriggerName = animationTriggerName;
        }
    }
}