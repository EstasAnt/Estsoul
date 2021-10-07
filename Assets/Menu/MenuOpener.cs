using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuOpener : BaseMenu
{
    public float lastTimeScale = 1;
    public List<GameObject> backGrounds;

    public override void SwitchTo(BaseMenu menu)
    {
        enabled = false;
        menu.gameObject.SetActive(true);
        foreach(GameObject image in backGrounds) image.SetActive(true);
        lastTimeScale = Time.timeScale;
        Time.timeScale = 0;
    }
}
