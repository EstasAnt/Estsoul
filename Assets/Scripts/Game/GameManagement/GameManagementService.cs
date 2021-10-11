using Core.Services;
using Core.Services.SceneManagement;
using Game.Items;
using Tools.Unity;
using UnityDI;
using UnityEngine;

namespace Game.GameManagement
{
    public class GameManagementService : ILoadableService, IUnloadableService
    {
        [Dependency]
        private UnityEventProvider unityEventProvider;
        [Dependency] 
        private readonly SceneManagerService _SceneManagerService;
        [Dependency] 
        private readonly ItemsService _ItemsService;
        [Dependency]
        private MenuOpener menu
        {
            get
            {
                return ContainerHolder.Container.Resolve<MenuOpener>();
            }
        }

        public void Load()
        {
            unityEventProvider.OnUpdate += CheckForPauseButton;
            Debug.Log("here");
        }

        public void Unload()
        {
            unityEventProvider.OnUpdate -= CheckForPauseButton;
        }

        public void RestartGame()
        {
            _ItemsService.ClearAllItems();
            _SceneManagerService.LoadScene(SceneType.GameLevel_1, true);
        }

        public void CheckForPauseButton()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                menu.Pause();
            }
        }
    }
}