using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : Singleton<MenuManager>
{
    [SerializeField] private Image mask;
    [SerializeField] private Transform mainMenuRoot;
    [SerializeField] private Transform contractMenuRoot;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button contractsButton;
    [SerializeField] private Button contractsBackButton;
    [SerializeField] private Button contractsPlayButton;
    [SerializeField] private float menuTransitionHalftime = 0.5f;

    private bool _isMenuTransitioning;

    private void Start()
    {
        contractsButton.onClick.AddListener(MaybeShowContractMenu);
        contractsBackButton.onClick.AddListener(MaybeShowMainMenu);
        quitButton.onClick.AddListener(Application.Quit);
        contractsPlayButton.onClick.AddListener(() => LoadLevel(0));
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void MaybeShowContractMenu()
    {
        if (!_isMenuTransitioning)
        {
            _isMenuTransitioning = true;
            TransitionMenu(ShowContractsMenu, () => _isMenuTransitioning = false);
        }
    }

    private void MaybeShowMainMenu()
    {
        if (!_isMenuTransitioning)
        {
            _isMenuTransitioning = true;
            TransitionMenu(ShowMainMenu, () => _isMenuTransitioning = false);
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

    private void TransitionMenu(Action onMaskChangeHalfway, Action onMaskChangeFinish, bool animateInAndOut = true)
    {
        Tweener tweenerIn = DOTween.To(OnChangeAlpha, 0, 1, menuTransitionHalftime);
        tweenerIn.onComplete = () =>
        {
            if (onMaskChangeHalfway != null)
            {
                onMaskChangeHalfway.Invoke();
            }

            if (animateInAndOut)
            {
                Tweener tweenerOut = DOTween.To(OnChangeAlpha, 1, 0, menuTransitionHalftime);
                if (onMaskChangeFinish != null)
                {
                    tweenerOut.onComplete = onMaskChangeFinish.Invoke;
                }
            }
        };
    }

    private void OnChangeAlpha(float value)
    {
        mask.color = new Color(0, 0, 0, value);
    }

    private void LoadLevel(int level)
    {
        _isMenuTransitioning = true;
        TransitionMenu(() => SceneManager.LoadScene("Level_1"), null, false); //TODO specific level loading
    }

    void OnSceneLoaded(Scene s, LoadSceneMode mode)
    {
        _isMenuTransitioning = false;
    }
}