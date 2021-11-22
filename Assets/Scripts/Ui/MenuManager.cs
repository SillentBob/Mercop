using System;
using System.Collections.Generic;
using Core;
using Core.Events;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : Singleton<MenuManager>
{
    // @formatter:off
    [SerializeField] private string mainMenuSceneName = "MainMenu"; //TODO change to some reference
    [SerializeField] private SceneMask mask;
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
    
    [Header("Pause Menu")]
    [SerializeField] private PauseMenu pauseMenuRoot;
    [SerializeField] private Button pauseMenuQuitButton;
    [SerializeField] private Button pauseMenuMainmenuButton;
    [SerializeField] private Button pauseMenuResumeButton;
    
    // @formatter:on

    private bool isMenuLocked;
    private Image maskImage;
    private ToggleGroup contractsToggleGroup;

    private const string FORMATTER_TAG_PLAYER = "{PLAYER}";

    protected override void Awake()
    {
        base.Awake();
        maskImage = mask.GetComponentInChildren<Image>();
        contractsToggleGroup = contractListContainer.GetComponent<ToggleGroup>();
        EventManager.AddListener(EventTypes.Pause, OnPauseChange);
    }

    private void Start()
    {
        RegisterMenuButtonEvents();
        RePopulateContractsList();
        ResolvePlayEnabled();
        
        ShowMainMenu();
        SetPlayerName(playerNameInput.text);
    }

    private void RegisterMenuButtonEvents()
    {
        quitButton.onClick.AddListener(MaybeAnimateQuitGame);
        playerNameInput.onValueChanged.AddListener(SetPlayerName);
        contractsButton.onClick.AddListener(MaybeAnimateShowContractMenu);

        contractsBackButton.onClick.AddListener(MaybeAnimateShowMainMenu);
        contractsPlayButton.onClick.AddListener(LoadSelectedLevel);

        pauseMenuQuitButton.onClick.AddListener(MaybeAnimateQuitGame);
        pauseMenuMainmenuButton.onClick.AddListener(LoadMainMenu);
        pauseMenuResumeButton.onClick.AddListener(ResumeGameplay);
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

    private void SetPlayerName(String name)
    {
        GameManager.Instance.playerSettings.playerName = name;
    }

    private void MaybeAnimateShowContractMenu()
    {
        MaybeAnimateShowMenu(ShowContractsMenu);
    }

    private void MaybeAnimateShowMainMenu()
    {
        MaybeAnimateShowMenu(ShowMainMenu);
    }

    private void MaybeAnimateQuitGame()
    {
        MaybeAnimateShowMenu(Application.Quit);
    }

    private void MaybeAnimateShowMenu(Action menuAction)
    {
        if (!isMenuLocked)
        {
            isMenuLocked = true;
            TransitionMenu(menuAction, () => isMenuLocked = false);
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

    private void ShowPauseMenu(bool show)
    {
        pauseMenuRoot.gameObject.SetActive(show);
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
        tweenerIn.SetUpdate(true);
        if (onFinish != null)
        {
            tweenerIn.onComplete = onFinish.Invoke;
        }
    }

    private void AnimateMaskOut(Action onFinish)
    {
        Tweener tweenerOut = DOTween.To(OnChangeAlpha, 1, 0, menuTransitionHalftime);
        tweenerOut.SetUpdate(true);
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
        maskImage.color = new Color(maskImage.color.r, maskImage.color.g, maskImage.color.b, value);
    }

    private void LoadSelectedLevel()
    {
        MaybeAnimateShowMenu(() =>
            {
                SceneManager.LoadScene(GameManager.Instance.selectedContract.sceneName, LoadSceneMode.Single); // single??
                PlayerGuiManager.Instance.ShowGui(true);
                mainMenuRoot.gameObject.SetActive(false);
                contractMenuRoot.gameObject.SetActive(false);
            }
        );
    }

    private void LoadMainMenu()
    {
        MaybeAnimateShowMenu(() => SceneManager.LoadScene(mainMenuSceneName));
    }

    private void ResumeGameplay()
    {
        EventManager.Invoke(EventTypes.Pause, new PauseEvent(false));
    }

    private void OnPauseChange(PauseEvent evt)
    {
        var show = evt.isPaused;
        MaybeAnimateShowMenu(() => ShowPauseMenu(show));
    }
}