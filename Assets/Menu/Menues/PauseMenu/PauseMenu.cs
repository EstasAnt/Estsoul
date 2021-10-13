using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameManagement;
using UI;
using UnityDI;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : BaseMenu
{
    [Dependency] private readonly GameManagementService _GameManagementService;
    public MenuOpener opener;
    public Button RestartButton;

    private void Awake()
    {
        RestartButton.onClick.AddListener(RestartGame);
    }

    protected override void Start()
    {
        base.Start();
        opener = transform.parent.gameObject.GetComponent<MenuOpener>();
    }

    public override void Return(MenuActionSignal signal)
    {
        _signalBus.UnSubscribe<MenuActionSignal>(this);
        opener.Continue();
    }

    private void RestartGame()
    {
        Return(new MenuActionSignal());
        _GameManagementService.RestartGame();
    }

    public void Exit()
    {
        Application.Quit();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}
