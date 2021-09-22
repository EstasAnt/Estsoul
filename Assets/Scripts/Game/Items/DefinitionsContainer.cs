using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Items
{
    public class DefinitionsContainer : MonoBehaviour
    {
        [SerializeField]
        private List<ItemDef> _ItemDefs;
        public IReadOnlyList<ItemDef> ItemDefs => _ItemDefs;
        public IReadOnlyDictionary<string, ItemDef> ItemDefsDict => _ItemDefsDict;
        private Dictionary<string, ItemDef> _ItemDefsDict;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            _ItemDefsDict = _ItemDefs.ToDictionary(_=>_.ItemId);
        }
    }
}