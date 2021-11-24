using System;
using System.Collections.Generic;
using Mercop.Core;
using Mercop.Core.Events;
using Mercop.Core.Extensions;
using Mercop.Mission;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mercop.Ui
{
    public class MainMenuManager : Singleton<MainMenuManager>
    {
        // @formatter:off
        [Header("Main Menu")] 
        [SerializeField] private MainMenu mainMenuParent;
        [SerializeField] private Transform submenuMain;
        [SerializeField] private Button quitButton;
        [SerializeField] private Button contractsButton;
        [SerializeField] private TMP_InputField playerNameInput;

        [Header("Contracts Menu")]
        [SerializeField] private Transform submenuContracts;
        [SerializeField] private Button contractsBackButton;
        [SerializeField] private Button contractsPlayButton;
        [SerializeField] private Toggle contractSelectPrefab;
        [SerializeField] private Transform contractListContainer;
        [SerializeField] private TextMeshProUGUI contractDesctiptionHeader;
        [SerializeField] private TextMeshProUGUI contractDesctiption;
        [SerializeField] private TextMeshProUGUI contractRewardMoney;
        [SerializeField] private TextMeshProUGUI contractRewardReputation;
        // @formatter:on

        private ToggleGroup contractsToggleGroup;
        private const string FORMATTER_TAG_PLAYER = "{PLAYER}";
        private List<MissionContractAttributes> allContracts;

        protected override void Awake()
        {
            base.Awake();
            contractsToggleGroup = contractListContainer.GetComponent<ToggleGroup>();
        }

        private void Start()
        {
            RegisterMenuButtonEvents();
            //RePopulateContractsList();
            ResolvePlayEnabled();

            EnableView(true);
            ToggleMainMenu();
            SetPlayerName(playerNameInput.text);
        }

        private void RegisterMenuButtonEvents()
        {
            quitButton.onClick.AddListener(QuitGame);
            playerNameInput.onValueChanged.AddListener(SetPlayerName);
            contractsButton.onClick.AddListener(MaybeAnimateShowContractMenu);

            contractsBackButton.onClick.AddListener(MaybeAnimateShowMainMenu);
            contractsPlayButton.onClick.AddListener(LoadSelectedLevel);
        }

        private void RePopulateContractsList()
        {
            contractListContainer.ClearChildren();
            allContracts = GameManager.Instance.getAvailableContracts();
            SetSelectedContract(null);
            if (allContracts.Count == 0)
            {
                return;
            }

            for (int i = 0; i < allContracts.Count; i++)
            {
                MissionContractAttributes contract = allContracts[i];
                Toggle toggle = Instantiate(contractSelectPrefab, contractListContainer);
                toggle.group = contractsToggleGroup;
                toggle.GetComponentInChildren<TextMeshProUGUI>().SetText(contract.contractName);
                toggle.onValueChanged.AddListener(isChecked =>
                {
                    if (isChecked)
                    {
                        SetSelectedContract(contract);
                    }
                });
            }

            SelectFirstAvailableContract();
        }

        private void SelectFirstAvailableContract()
        {
            for (int i = 0; i < allContracts.Count; i++)
            {
                if (!allContracts[i].completed)
                {
                    SetSelectedContract(allContracts[i]);
                }
            }
        }

        private void ResolvePlayEnabled()
        {
            contractsPlayButton.interactable = GameManager.Instance.selectedContract != null;
        }

        private void SetSelectedContract(MissionContractAttributes contract)
        {
            GameManager.Instance.selectedContract = contract;
            ResolvePlayEnabled();
            contractDesctiption.SetText(contract != null ? GetFormattedDescription(contract.description) : "");
            contractDesctiptionHeader.SetText(contract != null ? contract.contractName : "");
            contractRewardMoney.SetText(contract != null ? $"{contract.reward.money} $" : "");
            contractRewardReputation.SetText(contract != null ? $"{contract.reward.reputation} REP" : "");
        }

        private string GetFormattedDescription(string description)
        {
            return description.Replace(FORMATTER_TAG_PLAYER, GameManager.Instance.playerSettings.playerName);
        }

        private void SetPlayerName(String playerName)
        {
            GameManager.Instance.playerSettings.playerName = playerName;
        }

        private void MaybeAnimateShowContractMenu()
        {
            ScreenMaskManager.Instance.MaybeAnimateShowMask(ToggleContractsMenu);
        }

        private void MaybeAnimateShowMainMenu()
        {
            ScreenMaskManager.Instance.MaybeAnimateShowMask(ToggleMainMenu);
        }

        private void ToggleMainMenu()
        {
            submenuMain.gameObject.SetActive(true);
            submenuContracts.gameObject.SetActive(false);
        }

        private void ToggleContractsMenu()
        {
            RePopulateContractsList();
            submenuContracts.gameObject.SetActive(true);
            submenuMain.gameObject.SetActive(false);
        }

        private void EnableView(bool enable)
        {
            mainMenuParent.gameObject.SetActive(enable);
        }

        private void LoadSelectedLevel()
        {
            ScreenMaskManager.Instance.MaybeAnimateShowMask(() =>
            {
                EventManager.Invoke( new LoadSceneEvent(GameManager.Instance.selectedContract.sceneName, false, true, true));
                PlayerGuiManager.Instance.ShowGui(true);
                EnableView(false);
            });
        }

        private void QuitGame()
        {
            EventManager.Invoke(new QuitGameEvent());
        }
    }
}