using System.Collections.Generic;
using Mercop.Audio;
using Mercop.Core;
using Mercop.Core.Events;
using Mercop.Core.Extensions;
using Mercop.Mission;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Mercop.Ui
{
    public class ContractsState : State
    {
        // @formatter:off
        [Header("Contracts Menu")] 
        [SerializeField] private Contracts contractsRoot;
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

        protected void Awake()
        {
            contractsToggleGroup = contractListContainer.GetComponent<ToggleGroup>();
            RegisterEventListeners();
            RegisterClickSounds();
        }

        private void RegisterEventListeners()
        {
            contractsBackButton.onClick.AddListener(StatesManager.Instance.PreviousState);
            contractsPlayButton.onClick.AddListener(LoadSelectedLevel);
        }
        
        private void RegisterClickSounds()
        {
            contractsBackButton.onClick.AddListener(()=>AudioPlayer.Instance.Play(AudioPlayer.Sound.UiClick));
            contractsPlayButton.onClick.AddListener(()=>AudioPlayer.Instance.Play(AudioPlayer.Sound.UiClick));
        }

        private void ResolvePlayEnabled()
        {
            contractsPlayButton.interactable = GameManager.Instance.selectedContract != null;
        }

        private string GetFormattedDescription(string description)
        {
            return description.Replace(FORMATTER_TAG_PLAYER, GameManager.Instance.playerSettings.playerName);
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

        private void LoadSelectedLevel()
        {
            StatesManager.Instance.LoadState<PlayerGameGuiState>(() => EventManager.Invoke(new LoadSceneEvent(
                GameManager.Instance.selectedContract.sceneName, false, true,
                true)));
        }

        private void EnableView(bool enable)
        {
            contractsRoot.gameObject.SetActive(enable);
        }

        public override void OnStateEnter()
        {
            RePopulateContractsList();
            EnableView(true);
        }

        public override void OnStateExit()
        {
            EnableView(false);
        }
    }
}