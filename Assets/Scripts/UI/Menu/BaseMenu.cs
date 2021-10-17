using InControl;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityDI;
using UnityEngine;
using UnityEngine.Events;
using Character.Control;
using KlimLib.SignalBus;

public class BaseMenu : MonoBehaviour
{
    [Dependency] protected readonly SignalBus _signalBus;

    public BaseMenu parent;

    protected virtual void Start()
    {
        ContainerHolder.Container.BuildUp(GetType(), this);
    }

    public virtual void Return(PlayerActionWasPressedSignal signal)
    {
        if (signal.PlayerAction != UniversalPlayerActions.Return) return;
        SwitchTo(parent);
    }

    public virtual void SwitchTo(BaseMenu menu)
    {
        menu.gameObject.SetActive(true);
        gameObject.SetActive(false);
        _signalBus.UnSubscribe<PlayerActionWasPressedSignal>(this);
        _signalBus.Subscribe<PlayerActionWasPressedSignal>(menu.Return, menu);
    }

    protected virtual void OnDestroy() { }
}