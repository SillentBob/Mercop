using System;
using Mercop.Core;
using Mercop.Core.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Mercop.Ui
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
            EventManager.AddListener<PauseEvent>(OnPauseChange);
        }

        private void RegisterMenuButtonEvents()
        {
            pauseMenuQuitButton.onClick.AddListener(QuitGame);
            pauseMenuMainmenuButton.onClick.AddListener(LoadMainMenu);
            pauseMenuResumeButton.onClick.AddListener(() => ResumeGameplay());
        }

        private void QuitGame()
        {
            EventManager.Invoke(new QuitGameEvent());
        }

        private void ResumeGameplay(bool animateMask = true)
        {
            Action resumeAction = () =>
            {
                ShowPauseMenu(false);
                EventManager.Invoke(new PauseEvent(false));
            };

            if (animateMask)
            {
                ScreenMaskManager.Instance.MaybeAnimateShowMask(resumeAction.Invoke);
            }
            else
            {
                resumeAction.Invoke();
            }
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
                ResumeGameplay(false);
                EventManager.Invoke(new LoadSceneEvent(null, true, true, false));
            });
        }
    }
}