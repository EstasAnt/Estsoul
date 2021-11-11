using KlimLib.SignalBus;
using System.Collections;
using System.Collections.Generic;
using UnityDI;
using UnityEngine;
using UnityEngine.UI;
using Character.Control;

public class MenuOpener : BaseMenu
{
    public float lastTimeScale = 1;
    public List<GameObject> backGrounds;
    PauseMenu pauseMenu; 

    void Awake()
    {
        ContainerHolder.Container.BuildUp(this);
        ContainerHolder.Container.RegisterInstance(this);
        pauseMenu = GetComponentInChildren<PauseMenu>(true);
    }

    protected override void Start()
    {
        base.Start();
        Debug.Log("here1");
        gameObject.SetActive(false);
        _signalBus.Subscribe<PlayerActionWasPressedSignal>(Pause, this);
    }

    public override void SwitchTo(BaseMenu menu)
    {
        menu.gameObject.SetActive(true);
        foreach(GameObject image in backGrounds) image.SetActive(true);
        lastTimeScale = Time.timeScale;
        Time.timeScale = 0;
        _signalBus.UnSubscribe<PlayerActionWasPressedSignal>(this);
        _signalBus.Subscribe<PlayerActionWasPressedSignal>(menu.Return, menu);
    }

    public void Pause(PlayerActionWasPressedSignal signal)
    {
        if (signal.PlayerAction != UniversalPlayerActions.Return) return;
        _signalBus.UnSubscribe<PlayerActionWasPressedSignal>(this);
        enabled = false;
        gameObject.SetActive(true);
        SwitchTo(pauseMenu);
        _signalBus.FireSignal(new PauseGameSignal{Pause = true});
    }

    public void Continue()
    {
        _signalBus.Subscribe<PlayerActionWasPressedSignal>(Pause, this);
        enabled = true;
        gameObject.SetActive(true);
        pauseMenu.gameObject.SetActive(false);
        foreach (GameObject image in backGrounds) image.SetActive(false);
        Time.timeScale = lastTimeScale;
        _signalBus.FireSignal(new PauseGameSignal{Pause = false});
    }

    new private void OnDestroy()
    {
        Debug.Log("here");
        ContainerHolder.Container.Unregister<MenuOpener>();
        _signalBus.UnSubscribe<PlayerActionWasPressedSignal>(this);
    }
}
