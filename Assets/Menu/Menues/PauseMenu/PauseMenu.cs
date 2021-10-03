using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : BaseMenu
{
    public void Leave()
    {
        transform.parent.gameObject.GetComponent<MenuOpener>().enabled=true;
        Time.timeScale = transform.parent.gameObject.GetComponent<MenuOpener>().lastTimeScale;
        Destroy(gameObject);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
