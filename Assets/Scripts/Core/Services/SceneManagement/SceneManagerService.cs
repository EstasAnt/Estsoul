using System;
using System.Collections;
using System.Collections.Generic;
using Core.Services;
using KlimLib.TaskQueueLib;
using SceneManagement;
using Tools.Unity;
using UnityDI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Services.SceneManagement {
    public class SceneManagerService : ILoadableService {

        [Dependency]
        private readonly UnityEventProvider _EventProvider;

        private readonly Dictionary<SceneType, SceneLoadingParameters> _SceneLoadingParametersMap = new Dictionary<SceneType, SceneLoadingParameters>() {
            {
                SceneType.GameLevel_1, new GameLevelLoadingParameters()
            },
            {
                SceneType.LevelTemplate, new GameLevelLoadingParameters()
            },
            {
                SceneType.SpiritWorld, new SpiritWorldLoadingParameters()
            },
            {
                SceneType.SpiritWorldShort, new SpiritWorldLoadingParameters()
            },
            {
                SceneType.SpiritWorldLong, new SpiritWorldLoadingParameters()
            },
            {
                SceneType.BossFight_AncientToad, new GameLevelLoadingParameters()
            }
        };

        public SceneType ActiveScene {
            get {
                Enum.TryParse(SceneManager.GetActiveScene().name, false, out SceneType result);
                return result;
            }
        }

        public bool IsGameScene {
            get {
                var scene = ActiveScene;
                return true;
            }
        }

        public void Load() {

        }

        public void LoadScene(SceneType scene) {
            var activeScene = ActiveScene;
            if (activeScene == scene)
                return;
            var oldSceneParameters = _SceneLoadingParametersMap[activeScene];
            var newParameters = _SceneLoadingParametersMap[scene];
            oldSceneParameters.BeforeUnload();
            oldSceneParameters.UnloadingTasks.RunTasksListAsQueue(
                () => {
                    OnSceneUnloadSuccess(activeScene);
                    oldSceneParameters.AfterUnload();
                    newParameters.BeforeLoad();
                    SceneManager.LoadScene(scene.ToString());
                    newParameters.LoadingTasks.RunTasksListAsQueue(
                        () => {
                            OnSceneLoadSuccess(scene);
                            newParameters.AfterLoad();
                        },
                        (task, e) => {
                            OnSceneLoadFail(scene);
                            Debug.LogError($"{task} task failed with {e}");
                        },
                        null);
                },
                (task, e) => {
                    OnSceneUnloadFail(scene);
                    Debug.LogError($"{task} task failed with {e}");
                },
                null);
        }

        private void OnSceneUnloadSuccess(SceneType scene) {
            Debug.Log($"{scene} successfully unloaded");
        }
        private void OnSceneUnloadFail(SceneType scene) {
            Debug.LogError($"{scene} unload fail");
        }
        private void OnSceneLoadSuccess(SceneType scene) {
            Debug.Log($"{scene} successfully loaded");
        }
        private void OnSceneLoadFail(SceneType scene) {
            Debug.LogError($"{scene} load fail");
        }
    }

    public enum SceneType {
        GameLevel_1,
        SpiritWorld,
        LevelTemplate,
        SpiritWorldShort,
        SpiritWorldLong,
        BossFight_AncientToad,
    }
}
