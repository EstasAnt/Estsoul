using System;

namespace Game.Movement
{
    [Serializable]
    public class DontMoveAnimationInfo
    {
        public string AnimationName;
        public bool DontMoveInAir;
        public DontMoveAnimationInfo(string animationName, bool dontMoveInAir)
        {
            AnimationName = animationName;
            DontMoveInAir = dontMoveInAir;
        }
    }
}