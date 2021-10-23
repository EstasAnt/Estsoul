using System.Collections.Generic;
using Character.Health;
using Core.Audio;
using Core.Initialization.Base;
using Core.Initialization.Game;
using Core.Initialization.Items;
using Core.Services.Controllers;
using Core.Services.Game;
using Core.Services.SceneManagement;
using DebugTools;
using Game.GameManagement;
using Game.Items;
using KlimLib.ResourceLoader;
using KlimLib.SignalBus;
using KlimLib.TaskQueueLib;
using SceneManagement;
using SceneManagement.Game;
using UI.Markers;

namespace Core.Initialization {
    public static class InitializationParameters {
        public static List<Task> BaseTasks =>
            new List<Task> {
                new ContainerInitializationTask(),
                new BaseServiceInitializationTask<SignalBus, SignalBus>(),
                new BaseServiceInitializationTask<IResourceLoaderService, ResourceLoaderService>(),
                new UnityEventProviderRegisterTask(),
                new RegisterAndLoadServiceTask<SceneManagerService>(),
                new RegisterAndLoadServiceTask<ControllersStatusService>(),
                new RegisterAndLoadServiceTask<MarkerService>(),
                new RegisterAndLoadServiceTask<AudioService>(),
                new ItemsInitializeTask(),
                new RegisterAndLoadServiceTask<GameManagementService>(),
                new RegisterAndLoadServiceTask<DebugToolsService>(),
            };

        public static List<Task> BaseGameLoadTasks =>
            new List<Task> {
                 new WaitForAwakesTask(),
                 new GameCameraSpawnTask(),
                new RegisterAndLoadServiceTask<SceneLoadingService>(),
                new RegisterAndLoadServiceTask<HealthService>(),
                new RegisterAndLoadServiceTask<CharacterCreationService>(),
            };
        
        public static List<Task> BaseGameUnloadTasks =>
            new List<Task> {
                new UnregisterAndUnloadServiceTask<HealthService>(),
                new UnregisterAndUnloadServiceTask<CharacterCreationService>(),
                new UnregisterAndUnloadServiceTask<SceneLoadingService>(),
            };

        public static List<Task> GameLoadTasks => new List<Task> {
            new RegisterAndLoadServiceTask<SpiritWorldLoaderService>(),
        };

        public static List<Task> GameUnloadTasks => new List<Task>() {
            new UnregisterAndUnloadServiceTask<SpiritWorldLoaderService>(),
        };

        public static List<Task> SpiritWorldLoadTasks => new List<Task>()
        {
            new RegisterAndLoadServiceTask<UsualWorldLoaderService>(),
            new RegisterAndLoadServiceTask<SceneLoadingService>(),
        };
        
        public static List<Task> SpiritWorldUnloadTasks => new List<Task>() {
            new UnregisterAndUnloadServiceTask<UsualWorldLoaderService>(),
            new UnregisterAndUnloadServiceTask<SceneLoadingService>(),
        };
        
    }
}