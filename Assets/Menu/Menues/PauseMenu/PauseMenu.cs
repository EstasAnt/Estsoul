using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : BaseMenu
{
    public MenuOpener opener;

    private void Start()
    {
        opener = transform.parent.gameObject.GetComponent<MenuOpener>();
    }

    public void Leave()
    {
        opener.enabled=true;
        Time.timeScale = opener.lastTimeScale;
        opener.image.enabled = false;
        Destroy(gameObject);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
