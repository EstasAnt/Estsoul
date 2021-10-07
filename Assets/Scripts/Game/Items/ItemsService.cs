using System.Collections.Generic;
using Core.Services;
using KlimLib.SignalBus;
using UnityDI;
using UnityEngine;

namespace Game.Items
{
    public class ItemsService : ILoadableService, IUnloadableService
    {
        [Dependency] private readonly DefinitionsContainer _DefinitionsContainer;
        [Dependency] private readonly SignalBus _SignalBus;


        private Dictionary<string, int> _ItemsStorage;
        
        public void Load()
        {
            LoadItemsStorage();
        }

        public void Unload()
        {
            
        }

        private void LoadItemsStorage()
        {
            _ItemsStorage = new Dictionary<string, int>();
            foreach (var itemDef in _DefinitionsContainer.ItemDefs)
            {
                _ItemsStorage.Add(itemDef.ItemId, 0);
            }
        }

        public void SaveItems()
        {
            
        }

        public bool ItemExists(string itemId)
        {
            return _DefinitionsContainer.ItemDefsDict.ContainsKey(itemId);
        }

        public int ItemsAmount(string itemId)
        {
            if(!ItemExists(itemId))
                return 0;
            return _ItemsStorage[itemId];
        }
        
        public void GetItem(string itemId, int amount)
        {
            if (!ItemExists(itemId))
            {
                Debug.LogError($"Item with id - '{itemId}' doesnt exist");
                return;   
            }
            if (amount <= 0)
            {
                Debug.LogError($"Trying to add negative or zero '{itemId}' items amount {amount}");
                return;
            }
            if (!_ItemsStorage.ContainsKey(itemId))
                _ItemsStorage.Add(itemId, 0);
            _ItemsStorage[itemId] += amount;
            _SignalBus.FireSignal(new ItemAmountChangedSignal(itemId, ItemsAmount(itemId)));
        }

        public bool ItemsEnough(string itemId, int amount)
        {
            if (!ItemExists(itemId))
                return false;
            if (!_ItemsStorage.ContainsKey(itemId))
                return false;
            return _ItemsStorage[itemId] >= amount;
        }
        
        public bool SpentItem(string itemId, int amount)
        {
            if (!ItemExists(itemId))
                return false;
            if (!ItemsEnough(itemId, amount))
                return false;
            _ItemsStorage[itemId] -= amount;
            _SignalBus.FireSignal(new ItemAmountChangedSignal(itemId, ItemsAmount(itemId)));
            return true;
        }

        public void ClearAllItems()
        {
            LoadItemsStorage();
        }
    }
}