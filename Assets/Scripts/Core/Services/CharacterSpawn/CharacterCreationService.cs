using System.Collections.Generic;
using Character.Control;
using Com.LuisPedroFonseca.ProCamera2D;
using Game.Match;
using InControl;
using KlimLib.ResourceLoader;
using KlimLib.SignalBus;
using UnityDI;
using UnityEngine;

namespace Core.Services.Game {
    public class CharacterCreationService : ILoadableService, IUnloadableService {

        [Dependency]
        private readonly SignalBus _SignalBus;
        [Dependency]
        private readonly IResourceLoaderService _ResourceLoader;
        [Dependency]
        private readonly PlayersSpawnSettings _PlayersSpawnSettings;

        private ProCamera2D Camera2D => _Camera2D == null ? ContainerHolder.Container.Resolve<ProCamera2D>() : _Camera2D;
        private readonly ProCamera2D _Camera2D;

        public void Load()
        {
            //ToDo: remove from here
            var playerData = new PlayerData(0, "MainCharacter", false, 0, "TestCharacter");

            var spawnPoint = _PlayersSpawnSettings.PlayerSpawnPoints[0].Point;
            CreateCharacter(playerData, spawnPoint.position);
        }

        public void Unload() {
            _SignalBus.UnSubscribeFromAll(this);
        }

        public CharacterUnit CreateCharacter(PlayerData playerData, Vector3 pos) {
            var path = Path.Resources.CharacterPath(playerData.CharacterId);
            var unit = _ResourceLoader.LoadResourceOnScene<CharacterUnit>(path, pos, Quaternion.identity);
            var playerController = unit.gameObject.AddComponent<Character.Control.PlayerController>();

#if UNITY_STANDALONE || UNITY_EDITOR
            InitializeStationaryInput(unit);
#endif
#if UNITY_ANDROID
            InitializeDeviceInput(unit);
#endif
            unit.Initialize(playerData.PlayerId, playerData.CharacterId);
            _SignalBus.FireSignal(new CharacterSpawnedSignal(unit));
            if(Camera2D != null) {
                Camera2D.AddCameraTarget(unit.transform);
            }
            return unit;
        }

        private void InitializeStationaryInput(CharacterUnit character)
        {
            character.gameObject.AddComponent<StationaryInputController>();
        }

        private void InitializeDeviceInput(CharacterUnit character)
        {
            character.gameObject.AddComponent<DeviceInputController>();
        }
    }
}