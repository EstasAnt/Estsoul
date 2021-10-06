using InControl;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseMenu : MonoBehaviour
{
    [Serializable]
    private class MenuBind
    {
        public UnityEvent unityEvent;

        public List<KeyCode> playerActions;
    }



    [SerializeField] List<MenuBind> binds = new List<MenuBind>();

    private void Update()
    {
        foreach (MenuBind bind in binds)
        {
            foreach(KeyCode key in bind.playerActions)
            {
                if (Input.GetKeyDown(key))
                {
                    print("bind acted in " + gameObject);
                    bind.unityEvent.Invoke();
                    break;
                }
            }
        }
    }

    public virtual void SwitchTo(BaseMenu menu)
    {
        menu.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}