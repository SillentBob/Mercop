using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : Singleton<MenuManager>
{
    // @formatter:off
    [SerializeField] private Canvas mask;
    [SerializeField] private float menuTransitionHalftime = 0.5f;
    
    [Header("Main Menu")] 
    [SerializeField] private Transform mainMenuRoot;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button contractsButton;
    [SerializeField] private TMP_InputField playerNameInput;

    [Header("Contracts Menu")]
    [SerializeField] private Transform contractMenuRoot;
    [SerializeField] private Button contractsBackButton;
    [SerializeField] private Button contractsPlayButton;
    [SerializeField] private List<MissionContractAttributes> allContracts;
    [SerializeField] private Toggle contractSelectPrefab;
    [SerializeField] private Transform contractListContainer;
    [SerializeField] private TextMeshProUGUI contractDesctiptionHeader;
    [SerializeField] private TextMeshProUGUI contractDesctiption;
    [SerializeField] private TextMeshProUGUI contractRewardMoney;
    [SerializeField] private TextMeshProUGUI contractRewardReputation;
     
    // @formatter:on

    private bool _isMenuLocked;
    private Image _maskImage;
    private ToggleGroup _contractsToggleGroup;

    private const string FORMATTER_TAG_PLAYER = "{PLAYER}";

    protected override void Awake()
    {
        base.Awake();
        _maskImage = mask.GetComponentInChildren<Image>();
        _contractsToggleGroup = contractListContainer.GetComponent<ToggleGroup>();
        RegisterMenuButtonEvents();
        RePopulateContractsList();
        ResolvePlayEnabled();
    }

    private void Start()
    {
        ShowMainMenu();
        SetPlayerName(playerNameInput.text);
    }

    private void RegisterMenuButtonEvents()
    {
        contractsButton.onClick.AddListener(MaybeAnimateShowContractMenu);
        contractsBackButton.onClick.AddListener(MaybeAnimateShowMainMenu);
        quitButton.onClick.AddListener(Application.Quit);
        contractsPlayButton.onClick.AddListener(LoadLevel);
        playerNameInput.onValueChanged.AddListener(SetPlayerName);
    }

    private void RePopulateContractsList()
    {
        SetSelectedContract(null);
        if (allContracts.Count == 0)
        {
            return;
        }

        for (int i = 0; i < allContracts.Count; i++)
        {
            MissionContractAttributes contract = allContracts[i];
            Toggle toggle = Instantiate(contractSelectPrefab, contractListContainer);
            toggle.group = _contractsToggleGroup;
            toggle.GetComponentInChildren<TextMeshProUGUI>().SetText(contract.name);
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
        contractDesctiptionHeader.SetText(contract != null ? contract.name : "");
        contractRewardMoney.SetText(contract != null ? $"{contract.reward.money} $" : "");
        contractRewardReputation.SetText(contract != null ? $"{contract.reward.reputation} REP" : "");
    }

    private string GetFormattedDescription(string description)
    {
        return description.Replace(FORMATTER_TAG_PLAYER, GameManager.Instance.playerSettings.playerName);
    }

    private void SetPlayerName(String name)
    {
        GameManager.Instance.playerSettings.playerName = name;
    }

    private void MaybeAnimateShowContractMenu()
    {
        if (!_isMenuLocked)
        {
            _isMenuLocked = true;
            TransitionMenu(ShowContractsMenu, () => _isMenuLocked = false);
        }
    }

    private void MaybeAnimateShowMainMenu()
    {
        if (!_isMenuLocked)
        {
            _isMenuLocked = true;
            TransitionMenu(ShowMainMenu, () => _isMenuLocked = false);
        }
    }

    private void ShowMainMenu()
    {
        mainMenuRoot.gameObject.SetActive(true);
        contractMenuRoot.gameObject.SetActive(false);
    }

    private void ShowContractsMenu()
    {
        contractMenuRoot.gameObject.SetActive(true);
        mainMenuRoot.gameObject.SetActive(false);
    }

    private void TransitionMenu(Action onMaskChangeHalfway, Action onMaskChangeFinish)
    {
        AnimateMaskIn(() =>
        {
            if (onMaskChangeHalfway != null)
            {
                onMaskChangeHalfway.Invoke();
            }

            AnimateMaskOut(onMaskChangeFinish);
        });
    }

    private void AnimateMaskIn(Action onFinish)
    {
        EnableMaskObject(true);
        Tweener tweenerIn = DOTween.To(OnChangeAlpha, 0, 1, menuTransitionHalftime);
        if (onFinish != null)
        {
            tweenerIn.onComplete = onFinish.Invoke;
        }
    }

    private void AnimateMaskOut(Action onFinish)
    {
        Tweener tweenerOut = DOTween.To(OnChangeAlpha, 1, 0, menuTransitionHalftime);
        tweenerOut.onComplete = () =>
        {
            if (onFinish != null)
            {
                onFinish.Invoke();
            }

            EnableMaskObject(false);
        };
    }

    private void EnableMaskObject(bool enable)
    {
        mask.gameObject.SetActive(enable);
    }

    private void OnChangeAlpha(float value)
    {
        _maskImage.color = new Color(_maskImage.color.r, _maskImage.color.g, _maskImage.color.b, value);
    }

    private void LoadLevel()
    {
        _isMenuLocked = true;
        TransitionMenu(() => SceneManager.LoadScene(GameManager.Instance.selectedContract.sceneName),
            () => _isMenuLocked = false);
    }
}