using System.Collections;
using System.Collections.Generic;
using Core.Services.SceneManagement;
using Game.LevelSpecial;
using UnityEngine;

public class SceneLoadingTriggerInSignal : MonoBehaviour
{
    public ISceneLoadingRecation characterUnit;
    public SceneType SceneType;
    public float Delay;
    public SceneLoadingTriggerInSignal(ISceneLoadingRecation recation, SceneType sceneType, float delay)
    {
        SceneType = sceneType;
        Delay = delay;
    }
}
