using UnityEngine;

namespace Game.AI.CustomBehaviours.Behaviours
{
    public interface IMobsTarget
    {
        public Collider2D Collider { get; }
        public Transform Transform { get; }
    }
}