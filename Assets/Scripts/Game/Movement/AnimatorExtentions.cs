using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Movement
{
    public static class AnimatorExtensions
    {
        public static bool OneOfAnimationsIsPlaying(this Animator animator, IEnumerable<string> animationNames)
        {
            return animationNames != null &&
                   animationNames.Any(_ => animator.GetCurrentAnimatorStateInfo(0).IsName(_));
        }
    }
}