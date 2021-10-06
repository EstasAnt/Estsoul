using System.Collections.Generic;
using UnityEngine;

namespace Game.Character.Melee
{
    public class AttackGroup : MonoBehaviour
    {
        public List<Attack> Attacks;
        public string AnimationTriggerName;
        public float CoolDown;
        
        public float LastUseTime = float.NegativeInfinity;
    }
}