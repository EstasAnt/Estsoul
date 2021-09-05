using System.Collections.Generic;
using System.Linq;
using KlimLib.TaskQueueLib;

namespace Core.Initialization
{
    public class InitializerSpiritWorld : InitializerBase
    {
        protected override List<Task> SpecialTasks => InitializationParameters.BaseTasks
            .Concat(InitializationParameters.SpiritWorldLoadTasks).ToList();
    }
}