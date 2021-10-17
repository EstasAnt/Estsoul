using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameManagement;
using Character.Control;
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

    public override void Return(PlayerActionWasPressedSignal signal)
    {
        if (signal.PlayerAction != UniversalPlayerActions.Return) return;
        Return();
    }

    public void Return()
    {
        _signalBus.UnSubscribe<PlayerActionWasPressedSignal>(this);
        opener.Continue();
    }

    private void RestartGame()
    {
        Return(new PlayerActionWasPressedSignal());
        _GameManagementService.RestartGame();
    }

    public void Exit()
    {
        Application.Quit();
    }

    protected override void OnDestroy()
    {
        _signalBus.UnSubscribe<PlayerActionWasPressedSignal>(this);
        Time.timeScale = opener.lastTimeScale;
        base.OnDestroy();
    }
}
