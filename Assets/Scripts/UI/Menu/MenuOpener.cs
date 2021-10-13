using KlimLib.SignalBus;
using System.Collections;
using System.Collections.Generic;
using UnityDI;
using UnityEngine;
using UnityEngine.UI;
using UI;

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
        gameObject.SetActive(false);
        _signalBus.Subscribe<MenuActionSignal>(Pause, this);
    }

    public override void SwitchTo(BaseMenu menu)
    {
        menu.gameObject.SetActive(true);
        foreach(GameObject image in backGrounds) image.SetActive(true);
        lastTimeScale = Time.timeScale;
        Time.timeScale = 0;
        _signalBus.UnSubscribe<MenuActionSignal>(this);
        _signalBus.Subscribe<MenuActionSignal>(menu.Return, menu);
    }

    public void Pause(MenuActionSignal signal)
    {
        _signalBus.UnSubscribe<MenuActionSignal>(this);
        enabled = false;
        gameObject.SetActive(true);
        SwitchTo(pauseMenu);
    }

    public void Continue()
    {
        _signalBus.Subscribe<MenuActionSignal>(Pause, this);
        enabled = true;
        gameObject.SetActive(true);
        pauseMenu.gameObject.SetActive(false);
        foreach (GameObject image in backGrounds) image.SetActive(false);
        Time.timeScale = lastTimeScale;
    }

    new private void OnDestroy()
    {
        ContainerHolder.Container.Unregister<MenuOpener>();
        _signalBus.UnSubscribe<MenuActionSignal>(this);
    }
}
