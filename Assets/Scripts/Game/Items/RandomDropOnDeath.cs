using System;
using System.Collections.Generic;
using Character.Health;
using KlimLib.ResourceLoader;
using UnityDI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Items
{
    public class RandomDropOnDeath : MonoBehaviour
    {
        [Dependency]
        private IResourceLoaderService _ResourceLoaderService;
        
        public List<RandomDropInfo> RandomDropInfos;
        
        private IDamageable _Idamageable;

        private void Awake()
        {
            ContainerHolder.Container.BuildUp(GetType(), this);
            _Idamageable = GetComponent<IDamageable>();
        }

        private void Start()
        {
            if (_Idamageable != null)
            {
                _Idamageable.OnKill += IdamageableOnOnKill;
            }
        }

        private void OnDestroy()
        {
            if(_Idamageable == null)
                return;
            _Idamageable.OnKill -= IdamageableOnOnKill;
        }

        private void IdamageableOnOnKill(IDamageable dmgbl, Damage dmg)
        {
            GenerateAndSpawnDrop();
        }

        private void GenerateAndSpawnDrop()
        {
            if(RandomDropInfos == null || RandomDropInfos.Count == 0)
                return;
            var dropInfos = new List<DropInfo>();
            foreach (var randomDropInfo in RandomDropInfos)
            {
                var chanceValue = Random.value;
                if(chanceValue > randomDropInfo.Chance)
                    continue;
                var amount = Random.Range(randomDropInfo.AmountRandom.x, randomDropInfo.AmountRandom.y);
                if(amount <= 0 )
                    continue;
                var drop = new DropInfo()
                {
                    ItemId = randomDropInfo.ItemId,
                    Amount = amount,
                };
                dropInfos.Add(drop);
            }
            if(dropInfos.Count == 0)
                return;
            var dropPrefab = _ResourceLoaderService.LoadResourceOnScene<ItemDrop>(Path.Resources.GetDropPath(), transform.position, Quaternion.identity);
            dropPrefab.Initialize(dropInfos);
        }
    }

    [Serializable]
    public class RandomDropInfo
    {
        public string ItemId;
        public Vector2Int AmountRandom;
        public float Chance;
    }
}