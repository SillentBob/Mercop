using Core;
using Core.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class PauseMenuManager : Singleton<PauseMenuManager>
    {
        // @formatter:off
        [SerializeField] private PauseMenu pauseMenuRoot;
        [SerializeField] private Button pauseMenuQuitButton;
        [SerializeField] private Button pauseMenuMainmenuButton;
        [SerializeField] private Button pauseMenuResumeButton;
        // @formatter:on

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
            if (evt.isPaused)
            {
                ScreenMaskManager.Instance.MaybeAnimateShowMask(() => ShowPauseMenu(true));
            }
        }

        private void ShowPauseMenu(bool show)
        {
            pauseMenuRoot.gameObject.SetActive(show);
        }

        private void LoadMainMenu()
        {
            ScreenMaskManager.Instance.MaybeAnimateShowMask(() =>
            {
                ShowPauseMenu(false);
                ResumeGameplay();
                EventManager.Invoke(EventTypes.LoadScene, evt: new LoadSceneEvent(null, true, true, false));
            });
        }
    }
}