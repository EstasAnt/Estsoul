using System.Collections;
using KlimLib.SignalBus;
using Tools.Unity;
using UnityDI;
using UnityEngine;

namespace Core.Services.SceneManagement
{
    public class SceneLoadingService : ILoadableService, IUnloadableService
    {
        [Dependency] private readonly UnityEventProvider _EventProvider;
        [Dependency] private readonly SignalBus _SignalBus;
        [Dependency] private readonly SceneManagerService _SceneManagerService;

        private Coroutine _LoadCoroutine;
        
        public void Load()
        {
            _SignalBus.Subscribe<SceneLoadingTriggerInSignal>(OnSceneLoadingTriggerInSignal, this);
        }

        public void Unload()
        {
            if(_LoadCoroutine != null)
                _EventProvider.StopCoroutine(_LoadCoroutine);
            _SignalBus.UnSubscribeFromAll(this);
        }

        private void OnSceneLoadingTriggerInSignal(SceneLoadingTriggerInSignal signal)
        {
            if(_LoadCoroutine != null)
                return;
            _LoadCoroutine = _EventProvider.StartCoroutine(SceneLoadRoutine(signal.SceneType, signal.Delay));
        }

        private IEnumerator SceneLoadRoutine(SceneType sceneType, float delay)
        {
            yield return new WaitForSeconds(delay);
            _SceneManagerService.LoadScene(sceneType);
        }
    }
}