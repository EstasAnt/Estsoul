using Character.Control;
using Core.Services;
using KlimLib.SignalBus;
using System.Collections;
using System.Collections.Generic;
using UnityDI;
using UnityEngine;

namespace DebugTools
{
    public class DebugToolsService : ILoadableService, IUnloadableService
    {
        [Dependency] private readonly SignalBus _signalBus;
#if FAST_SKIP_ENABLED
        [Dependency] private TPPointsList pointsList;
#endif

        public void Load()
        {
            ContainerHolder.Container.RegisterInstance(this);
#if FAST_SKIP_ENABLED
            _signalBus.Subscribe<SceneChangedSignal>(ResetPointsSource, this);
            _signalBus.Subscribe<PlayerActionWasPressedSignal>(FastSkip, this);
#endif
        }
#if FAST_SKIP_ENABLED
        public void ResetPointsSource(SceneChangedSignal signal)
        {
            pointsList = ContainerHolder.Container.Resolve<TPPointsList>();
        }

        public void FastSkip(PlayerActionWasPressedSignal signal)
        {
            if (signal.PlayerAction == UniversalPlayerActions.Teleport)
            {
                pointsList.character.transform.position= pointsList.GetNextV2();
            }
        }
#endif
        public void Unload()
        {
            _signalBus.UnSubscribe<SceneChangedSignal>(this);
            ContainerHolder.Container.Unregister<DebugToolsService>();
        }

        public void SkipPoint()
        {

        }
    }
}