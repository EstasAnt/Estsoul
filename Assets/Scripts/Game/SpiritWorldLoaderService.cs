using System.Collections;
using Character.Health;
using Core.Services;
using Core.Services.SceneManagement;
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
        }

        private void OnCharacterDeadSignal(CharacterDeathSignal signal)
        {
            _UnityEventProvider.StartCoroutine(LoadSpiritWorldRoutine());
        }

        private IEnumerator LoadSpiritWorldRoutine()
        {
            yield return new WaitForSecondsRealtime(SpiritWordlLoadTime);
            _SceneManagerService.LoadScene(SceneType.SpiritWorld);
        }

        public void StopLoading()
        {
            _UnityEventProvider.StopCoroutine(LoadSpiritWorldRoutine());
        }
        
        public float SpiritWordlLoadTime => 3f;


        public void Unload()
        {
            _SignalBus.UnSubscribeFromAll(this);
        }
    }
}