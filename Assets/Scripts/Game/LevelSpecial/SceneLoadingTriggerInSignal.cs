using System.Collections;
using System.Collections.Generic;
using Core.Services.SceneManagement;
using UnityEngine;

public class SceneLoadingTriggerInSignal : MonoBehaviour
{
    public CharacterUnit characterUnit;
    public SceneType SceneType;
    public float Delay;
    public SceneLoadingTriggerInSignal(CharacterUnit characterUnit, SceneType sceneType, float delay)
    {
        SceneType = sceneType;
        Delay = delay;
    }
}
