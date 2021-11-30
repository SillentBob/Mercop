using System;
using Mercop.Audio;
using Mercop.Core;
using Mercop.Core.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Mercop.Ui
{
    public class PauseMenuView : View
    {
        // @formatter:off
        [SerializeField] private PauseMenu pauseMenuRoot;
        [SerializeField] private Button pauseMenuQuitButton;
        [SerializeField] private Button pauseMenuMainmenuButton;
        [SerializeField] private Button pauseMenuResumeButton;
        // @formatter:on

        protected void Awake()
        {
            RegisterMenuButtonEvents();
            RegisterClickSounds();
        }

        private void RegisterMenuButtonEvents()
        {
            pauseMenuQuitButton.onClick.AddListener(QuitGame);
            pauseMenuMainmenuButton.onClick.AddListener(LoadMainMenu);
            pauseMenuResumeButton.onClick.AddListener(() => ResumeGameplay());
        }
        
        private void RegisterClickSounds()
        {
            pauseMenuQuitButton.onClick.AddListener(() => AudioPlayer.Instance.Play(AudioPlayer.Sound.UiClick));
            pauseMenuMainmenuButton.onClick.AddListener(() => AudioPlayer.Instance.Play(AudioPlayer.Sound.UiClick));
            pauseMenuResumeButton.onClick.AddListener(() => AudioPlayer.Instance.Play(AudioPlayer.Sound.UiClick));
        }

        private void QuitGame()
        {
            EventManager.Invoke(new QuitGameEvent());
        }

        private void ResumeGameplay()
        {
            ViewManager.Instance.ShowView<PlayerGuiView>();
        }
        
        private void ShowPauseMenu(bool show)
        {
            pauseMenuRoot.gameObject.SetActive(show);
        }

        private void LoadMainMenu()
        {
            ViewManager.Instance.ShowView<MainMenuView>();
        }

        public override void OnShow()
        {
            ShowPauseMenu(true);
            EventManager.Invoke(new PauseEvent(true));
        }

        public override void OnHide()
        {
            ShowPauseMenu(false);
            EventManager.Invoke(new PauseEvent(false));
        }
    }
}