using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : Singleton<MenuManager>
{
    [SerializeField] private Canvas mask;

    [SerializeField] private Transform mainMenuRoot;
    [SerializeField] private Transform contractMenuRoot;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button contractsButton;
    [SerializeField] private Button contractsBackButton;
    [SerializeField] private Button contractsPlayButton;
    [SerializeField] private float menuTransitionHalftime = 0.5f;

    private bool _isMenuTransitioning;
    private Image maskImage;

    private void Start()
    {
        maskImage = mask.GetComponentInChildren<Image>();
        contractsButton.onClick.AddListener(MaybeShowContractMenu);
        contractsBackButton.onClick.AddListener(MaybeShowMainMenu);
        quitButton.onClick.AddListener(Application.Quit);
        contractsPlayButton.onClick.AddListener(() => LoadLevel(0));
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
        maskImage.color = new Color(maskImage.color.r, maskImage.color.g, maskImage.color.b, value);
    }

    private void LoadLevel(int level)
    {
        _isMenuTransitioning = true;
        TransitionMenu(() => SceneManager.LoadScene("Level_1"), null); //TODO specific level loading
    }

}