using System.Collections.Generic;
using Core.Initialization;
using Core.Services.SceneManagement;
using KlimLib.TaskQueueLib;

namespace SceneManagement
{
    public class SpiritWorldLoadingParameters : SceneLoadingParameters
    {
        public override List<Task> LoadingTasks => InitializationParameters.SpiritWorldLoadTasks;
        public override List<Task> UnloadingTasks => InitializationParameters.SpiritWorldUnloadTasks;
    }
}