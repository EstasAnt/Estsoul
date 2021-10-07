using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameManagement;
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

    public void Leave()
    {
        opener.enabled = true;
        Time.timeScale = opener.lastTimeScale;
        foreach (GameObject image in opener.backGrounds) image.SetActive(false);
        gameObject.SetActive(false);
    }

    private void RestartGame()
    {
        Leave();
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
