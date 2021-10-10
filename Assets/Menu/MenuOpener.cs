using System.Collections;
using System.Collections.Generic;
using UnityDI;
using UnityEngine;
using UnityEngine.UI;

public class MenuOpener : BaseMenu
{
    public float lastTimeScale = 1;
    public List<GameObject> backGrounds;

    void Awake()
    {
        ContainerHolder.Container.RegisterInstance(this);
        gameObject.SetActive(false);
    }

    public override void SwitchTo(BaseMenu menu)
    {
        menu.gameObject.SetActive(true);
        foreach(GameObject image in backGrounds) image.SetActive(true);
        lastTimeScale = Time.timeScale;
        Time.timeScale = 0;
    }

    public void Pause()
    {
        if (!enabled) return;
        enabled = false;
        gameObject.SetActive(true);
        GetComponentInChildren<PauseMenu>(true).gameObject.SetActive(true);
        foreach (GameObject image in backGrounds) image.SetActive(true);
        lastTimeScale = Time.timeScale;
        Time.timeScale = 0;
    }

    public void Continue()
    {
        enabled = true;
        gameObject.SetActive(true);
        GetComponentInChildren<PauseMenu>(true).gameObject.SetActive(false);
        foreach (GameObject image in backGrounds) image.SetActive(false);
        Time.timeScale = lastTimeScale;
    }

    new private void OnDestroy()
    {
        ContainerHolder.Container.Unregister<MenuOpener>();
    }
}
