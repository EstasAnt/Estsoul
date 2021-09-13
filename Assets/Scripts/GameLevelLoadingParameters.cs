using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Initialization;
using Core.Services.SceneManagement;
using KlimLib.SignalBus;
using KlimLib.TaskQueueLib;
using SceneManagement.SpiritWorld;
using Tools.Unity;
using UnityDI;
using UnityEngine;

namespace SceneManagement
{
    public class GameLevelLoadingParameters : SceneLoadingParameters
    {
        public override List<Task> LoadingTasks => InitializationParameters.BaseGameLoadTasks.Concat(InitializationParameters.GameLoadTasks).ToList();
        public override List<Task> UnloadingTasks => InitializationParameters.BaseGameUnloadTasks.Concat(InitializationParameters.GameUnloadTasks).ToList();
    }
}