using Core.Initialization.Base;
using Game.Items;
using KlimLib.ResourceLoader;
using KlimLib.TaskQueueLib;
using Tools;
using UnityDI;

namespace Core.Initialization.Items
{
    public class ItemsInitializeTask : AutoCompletedTask
    {
        protected override void AutoCompletedRun()
        {
            var resLoader = ContainerHolder.Container.Resolve<IResourceLoaderService>();
            var defContainer = resLoader.LoadResourceOnScene<DefinitionsContainer>("Prefabs/Items/DefinitionsContainer");
            ContainerHolder.Container.RegisterInstance(defContainer);
            var itemsServiceLoad = new RegisterAndLoadServiceTask<ItemsService>();
            itemsServiceLoad.Run();
        }
    }
}