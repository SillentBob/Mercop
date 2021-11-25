using System;
using Mercop.Core;
using Mercop.Core.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mercop.Ui
{
    public class MainMenuView : View
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
        }

        private void Start()
        {
            SetPlayerName(playerNameInput.text);
        }

        private void RegisterEventListeners()
        {
            quitButton.onClick.AddListener(QuitGame);
            playerNameInput.onValueChanged.AddListener(SetPlayerName);
            contractsButton.onClick.AddListener(() => ViewManager.Instance.ShowView<ContractsView>());
            leaderboardsButton.onClick.AddListener(() => ViewManager.Instance.ShowView<LeaderboardsView>());
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

        public override void OnShow()
        {
            EnableView(true);
        }

        public override void OnHide()
        {
            EnableView(false);
        }
    }
}