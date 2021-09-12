using System.Collections;
using Core.Services;
using Core.Services.SceneManagement;
using KlimLib.SignalBus;
using SceneManagement.SpiritWorld;
using Tools.Unity;
using UnityDI;
using UnityEngine;

namespace SceneManagement
{
    public class UsualWorldLoaderService : ILoadableService, IUnloadableService
    {
        [Dependency] private readonly SignalBus _SignalBus;
        [Dependency] private readonly SceneManagerService _SceneManagerService;
        [Dependency] private readonly UnityEventProvider _UnityEventProvider;
        
        public void Load()
        {
            _SignalBus.Subscribe<SpiritWorldGateInSignal>(OnSpiritWorldGateInSignal, this);
        }

        private void  OnSpiritWorldGateInSignal(SpiritWorldGateInSignal signal)
        {
            if(!signal.In)
                return;
            LoadUsualWorld();
        }

        public void LoadUsualWorld()
        {
            _UnityEventProvider.StartCoroutine(LoadUsualWorldRoutine());
        }
        
        private IEnumerator LoadUsualWorldRoutine()
        {
            yield return new WaitForSecondsRealtime(UsualWordlLoadTime);
            _SceneManagerService.LoadScene(SceneType.GameLevel_1);
        }
        
        public float UsualWordlLoadTime => 3f;
        
        public void Unload()
        {
            _SignalBus.UnSubscribeFromAll(this);
        }
    }
}