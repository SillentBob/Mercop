using System;
using Mercop.Audio;
using Mercop.Core;
using Mercop.Core.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mercop.Ui
{
    public class MainMenuState : State
    {
        // @formatter:off
        [Header("Main Menu")] 
        [SerializeField] private MainMenu mainMenuParent;
        [SerializeField] private Button quitButton;
        [SerializeField] private Button contractsButton;
        [SerializeField] private TMP_InputField playerNameInput;
        [SerializeField] private Button leaderboardsButton;
        // @formatter:on

        private void Awake()
        {
            RegisterEventListeners();
            RegisterClickSounds();
        }

        private void Start()
        {
            SetPlayerName(playerNameInput.text);
        }

        private void RegisterEventListeners()
        {
            quitButton.onClick.AddListener(QuitGame);
            playerNameInput.onValueChanged.AddListener(SetPlayerName);
            contractsButton.onClick.AddListener(() => StatesManager.Instance.LoadState<ContractsState>());
            leaderboardsButton.onClick.AddListener(() => StatesManager.Instance.LoadState<LeaderboardsState>());
        }

        private void RegisterClickSounds()
        {
            quitButton.onClick.AddListener(() => AudioPlayer.Instance.Play(AudioPlayer.Sound.UiClick));
            contractsButton.onClick.AddListener(() => AudioPlayer.Instance.Play(AudioPlayer.Sound.UiClick));
            leaderboardsButton.onClick.AddListener(() => AudioPlayer.Instance.Play(AudioPlayer.Sound.UiClick));
        }

        private void SetPlayerName(String playerName)
        {
            GameManager.Instance.playerSettings.playerName = playerName;
        }

        private void EnableView(bool enable)
        {
            mainMenuParent.gameObject.SetActive(enable);
        }

        private void QuitGame()
        {
            EventManager.Invoke(new QuitGameEvent());
        }

        public override void OnStateEnter()
        {
            EnableView(true);
        }

        public override void OnStateExit()
        {
            EnableView(false);
        }
    }
}