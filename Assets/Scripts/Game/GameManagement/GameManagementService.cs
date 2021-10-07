using Core.Services;
using Core.Services.SceneManagement;
using Game.Items;
using UnityDI;

namespace Game.GameManagement
{
    public class GameManagementService : ILoadableService, IUnloadableService
    {
        [Dependency] 
        private readonly SceneManagerService _SceneManagerService;
        [Dependency] 
        private readonly ItemsService _ItemsService;
        
        
        public void Load()
        {
            
        }

        public void Unload()
        {
            
        }

        public void RestartGame()
        {
            _ItemsService.ClearAllItems();
            _SceneManagerService.LoadScene(SceneType.GameLevel_1, true);
        }
    }
}