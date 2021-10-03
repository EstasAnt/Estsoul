using System.Collections;
using System.Collections.Generic;
using Core.Audio;
using Core.Services.SceneManagement;
using Game.LevelSpecial;
using UnityDI;
using UnityEngine;

public class SceneLoadingTrigger : TriggerSignalBroadcaster<CharacterUnit, SceneLoadingTriggerInSignal>
{
    [Dependency] private readonly AudioService _AudioService;

    public SceneType SceneType;
    public float Delay;
    public string EnterSound;
    
    protected override SceneLoadingTriggerInSignal CreateSignal(CharacterUnit unit, bool inTrigger)
    {
        if (!string.IsNullOrEmpty(EnterSound))
            _AudioService.PlaySound3D(EnterSound, false, false, transform.position);
        return new SceneLoadingTriggerInSignal(unit, SceneType, Delay);
    }
}
