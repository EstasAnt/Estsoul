using System.Collections.Generic;
using UnityDI;
using UnityEngine;

namespace Game.Items
{
    public class ItemDrop : MonoBehaviour
    {
        [Dependency]
        private readonly ItemsService _ItemService;

        public List<DropInfo> Drop;

        private void Start()
        {
            ContainerHolder.Container.BuildUp(GetType(), this);
        }
        
        public void Initialize(List<DropInfo> drop)
        {
            Drop = drop;
        }

        public void Collect()
        {
            if (Drop == null || Drop.Count == 0)
            {
                Debug.LogError($"drop == null or empty. go name: {gameObject.name}");
                Destroy(gameObject);
                return;
            }
            foreach (var dropInfo in Drop)
            {
                _ItemService.GetItem(dropInfo.ItemId, dropInfo.Amount);
            }
            Destroy(gameObject);
        }
    }
}