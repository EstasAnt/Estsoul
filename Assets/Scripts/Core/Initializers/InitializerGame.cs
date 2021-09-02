using System.Collections;
using System.Collections.Generic;
using Core.Initialization;
using KlimLib.TaskQueueLib;
using UnityEngine;

namespace Core.Initialization
{
    public class InitializerGame : InitializerBase
    {
        protected override List<Task> SpecialTasks => InitializationParameters.BaseGameTasks;
    }
}
