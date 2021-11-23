using System;
using Core;
using Core.Events;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuManager : Singleton<PauseMenuManager>
{
    [Header("Pause Menu")] [SerializeField]
    private PauseMenu pauseMenuRoot;

    [SerializeField] private Button pauseMenuQuitButton;
    [SerializeField] private Button pauseMenuMainmenuButton;

    [SerializeField] private Button pauseMenuResumeButton;
    //[SerializeField] private string mainMenuSceneName = "MainMenu"; //TODO change to some reference

    protected override void Awake()
    {
        base.Awake();
        RegisterMenuButtonEvents();
        EventManager.AddListener(EventTypes.Pause, OnPauseChange);
    }

    private void RegisterMenuButtonEvents()
    {
        pauseMenuQuitButton.onClick.AddListener(QuitGame);
        pauseMenuMainmenuButton.onClick.AddListener(LoadMainMenu);
        pauseMenuResumeButton.onClick.AddListener(ResumeGameplay);
    }

    private void QuitGame()
    {
        EventManager.Invoke(EventTypes.Quit, new QuitGameEvent());
    }

    private void ResumeGameplay()
    {
        EventManager.Invoke(EventTypes.Pause, evt: new PauseEvent(false));
    }

    private void OnPauseChange(PauseEvent evt)
    {
        var show = evt.isPaused;
        ScreenMaskManager.Instance.MaybeAnimateShowMask(() => ShowPauseMenu(show));
    }

    private void ShowPauseMenu(bool show)
    {
        pauseMenuRoot.gameObject.SetActive(show);
    }

    private void LoadMainMenu()
    {
        EventManager.Invoke(EventTypes.LoadScene, evt: new LoadSceneEvent(true));
    }
}