using System;
using UnityEngine;

namespace Game.Character.Melee
{
    [CreateAssetMenu(fileName = "AttackInfoConfig", menuName = "Weapon/AttackInfoConfig")]
    [Serializable]
    public class AttackInfoConfig : ScriptableObject
    {
        public int GroupNum;
        public int AttackNum;
    }
}