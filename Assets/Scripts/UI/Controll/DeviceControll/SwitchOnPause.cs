using System;
using System.Collections;
using System.Collections.Generic;
using KlimLib.SignalBus;
using UnityDI;
using UnityEngine;

public class SwitchOnPause : MonoBehaviour
{
    [Dependency] private readonly SignalBus _SignalBus;
    private void Start()
    {
        ContainerHolder.Container.BuildUp(this);
        _SignalBus.Subscribe<PauseGameSignal>(OnGamePause, this);
    }

    private void OnGamePause(PauseGameSignal signal)
    {
        if (signal.Pause)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        _SignalBus.UnSubscribeFromAll(this);
    }
}
