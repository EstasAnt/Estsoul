using KlimLib.SignalBus;
using System.Collections;
using System.Collections.Generic;
using UnityDI;
using UnityEngine;
using UnityEngine.UI;
using UI;

public class MenuOpener : BaseMenu
{
    [Dependency] private readonly SignalBus _signalBus;
    public float lastTimeScale = 1;
    public List<GameObject> backGrounds;

    void Awake()
    {
        ContainerHolder.Container.BuildUp(this);
        ContainerHolder.Container.RegisterInstance(this);
    }

    protected override void Start()
    {
        base.Start();
        gameObject.SetActive(false);
        _signalBus.Subscribe<PauseSwitchedSignal>(Pause, this);
    }

    public override void SwitchTo(BaseMenu menu)
    {
        menu.gameObject.SetActive(true);
        foreach(GameObject image in backGrounds) image.SetActive(true);
        lastTimeScale = Time.timeScale;
        Time.timeScale = 0;
    }

    public void Pause(PauseSwitchedSignal signal)
    {
        _signalBus.UnSubscribe<PauseSwitchedSignal>(this);
        _signalBus.Subscribe<PauseSwitchedSignal>(Continue, this);
        enabled = false;
        gameObject.SetActive(true);
        GetComponentInChildren<PauseMenu>(true).gameObject.SetActive(true);
        foreach (GameObject image in backGrounds) image.SetActive(true);
        lastTimeScale = Time.timeScale;
        Time.timeScale = 0;
    }

    public void Continue(PauseSwitchedSignal signal)
    {
        _signalBus.UnSubscribe<PauseSwitchedSignal>(this);
        _signalBus.Subscribe<PauseSwitchedSignal>(Pause, this);
        enabled = true;
        gameObject.SetActive(true);
        GetComponentInChildren<PauseMenu>(true).gameObject.SetActive(false);
        foreach (GameObject image in backGrounds) image.SetActive(false);
        Time.timeScale = lastTimeScale;
    }

    new private void OnDestroy()
    {
        ContainerHolder.Container.Unregister<MenuOpener>();
        _signalBus.UnSubscribe<PauseSwitchedSignal>(this);
    }
}
