using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuOpener : BaseMenu
{
    public float lastTimeScale = 1;
    public Image image;

    public override void SwitchTo(BaseMenu menu)
    {
        Instantiate(menu.gameObject, transform);
        enabled = false;
        image.enabled = true;
        lastTimeScale = Time.timeScale;
        Time.timeScale = 0;
    }
}
