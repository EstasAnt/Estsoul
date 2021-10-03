using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOpener : BaseMenu
{
    public float lastTimeScale = 1;

    public override void SwitchTo(BaseMenu menu)
    {
        Instantiate(menu.gameObject, transform);
        enabled = false;
        lastTimeScale = Time.timeScale;
        Time.timeScale = 0;
    }
}
