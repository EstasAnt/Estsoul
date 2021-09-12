using System.Collections;
using System.Linq;
using Character.Health;
using Core.Services;
using Core.Services.SceneManagement;
using Game.LevelSpecial;
using KlimLib.SignalBus;
using Tools.Unity;
using UnityDI;
using UnityEngine;

namespace SceneManagement.Game
{
    public class SpiritWorldLoaderService : ILoadableService, IUnloadableService
    {
        [Dependency] private readonly SignalBus _SignalBus;
        [Dependency] private readonly SceneManagerService _SceneManagerService;
        [Dependency] private readonly UnityEventProvider _UnityEventProvider;
        
        public void Load()
        {
            _SignalBus.Subscribe<CharacterDeathSignal>(OnCharacterDeadSignal, this);
            _SignalBus.Subscribe<RealWorldGateInSignal>(OnRealWorldGateInSignal, this);
        }

        private void OnCharacterDeadSignal(CharacterDeathSignal signal)
        {
            LoadSpiritWorld(SceneType.SpiritWorldShort);
        }
        
        private void OnRealWorldGateInSignal(RealWorldGateInSignal signal)
        {
            LoadSpiritWorld(SceneType.SpiritWorldLong);
        }

        private void LoadSpiritWorld(SceneType sceneType)
        {
            var characterUnit = CharacterUnit.Characters.FirstOrDefault();
            if(characterUnit)
                characterUnit.MovementController.MovementBlock = true;
            _UnityEventProvider.StartCoroutine(LoadSpiritWorldRoutine(sceneType));
        }
        
        private IEnumerator LoadSpiritWorldRoutine(SceneType sceneType)
        {
            yield return new WaitForSecondsRealtime(SpiritWordlLoadTime);
            _SceneManagerService.LoadScene(sceneType);
        }

        // public void StopLoading()
        // {
        //     _UnityEventProvider.StopCoroutine(LoadSpiritWorldRoutine());
        // }
        
        public float SpiritWordlLoadTime => 3f;


        public void Unload()
        {
            _SignalBus.UnSubscribeFromAll(this);
        }
    }
}