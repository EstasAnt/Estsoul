using System.Collections;
using System.Collections.Generic;
using Core.Audio;
using Core.Services.SceneManagement;
using Game.LevelSpecial;
using UnityDI;
using UnityEngine;

public class SceneLoadingTrigger : TriggerSignalBroadcaster<ISceneLoadingRecation, SceneLoadingTriggerInSignal>
{
    [Dependency] private readonly AudioService _AudioService;

    public SceneType SceneType;
    public float Delay;
    public string EnterSound;

    private bool _CheckedIn = false;
    
    protected override SceneLoadingTriggerInSignal CreateSignal(ISceneLoadingRecation unit, bool inTrigger)
    {
        if (!_CheckedIn)
        {
            if (!string.IsNullOrEmpty(EnterSound))
                _AudioService.PlaySound3D(EnterSound, false, false, transform.position);
            _CheckedIn = true;
        }

        return new SceneLoadingTriggerInSignal(unit, SceneType, Delay);
    }
}
