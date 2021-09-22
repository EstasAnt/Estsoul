using System;
using System.Collections.Generic;
using System.Linq;
using Character.Control;
using KlimLib.SignalBus;
using UnityDI;
using UnityEngine;

namespace Game.Items
{
    public class DropCollector : MonoBehaviour
    {
        [SerializeField]
        private Collider2D _DropCollectTrigger;
        [Dependency]
        private readonly SignalBus _SignalBus;
        private void Start()
        {
            ContainerHolder.Container.BuildUp(GetType(), this);
            _SignalBus.Subscribe<PlayerActionWasPressedSignal>(OnPlayerActionWasPressedSignal, this);
        }

        private void OnPlayerActionWasPressedSignal(PlayerActionWasPressedSignal signal)
        {
            if(signal.PlayerAction.Name != "Action")
                return;
            TryToCollectItems();
        }

        private void OnDestroy()
        {
            _SignalBus.UnSubscribeFromAll(this);
        }

        private void TryToCollectItems()
        {
            var dropColliders = new List<Collider2D>();
            var itemcollidersCount = Physics2D.OverlapCollider(_DropCollectTrigger, new ContactFilter2D {layerMask = Layers.Masks.Drop},  dropColliders);
            //ToDo: overlap ignores layer mask here
            if(itemcollidersCount == 0)
                return;
            var colliders = dropColliders.ToList();
            foreach (var collider in colliders)
            {
                var drop = collider.GetComponent<ItemDrop>();
                if (drop == null)
                    continue;
                drop.Collect();
            }
        }
    }
}