using System;
using Core.Events;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class ScreenMaskManager : Singleton<ScreenMaskManager>
    {
        [SerializeField] private Image maskImage;
        [SerializeField] private float maskAnimationHalfTime = 0.5f;
        
        private bool isMaskAnimating;

        protected override void Awake()
        {
            base.Awake();
            EventManager.AddListener(EventTypes.Quit, OnGameQuit);
        }

        public void MaybeAnimateShowMask(Action onMaskInFinish)
        {
            if (!isMaskAnimating)
            {
                isMaskAnimating = true;
                AnimateMaskInAndOut(onMaskInFinish, () => isMaskAnimating = false);
            }
        }

        private void AnimateMaskInAndOut(Action onMaskChangeHalfway, Action onMaskChangeFinish)
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
            Tweener tweenerIn = DOTween.To(OnChangeAlpha, 0, 1, maskAnimationHalfTime);
            tweenerIn.SetUpdate(true);
            if (onFinish != null)
            {
                tweenerIn.onComplete = onFinish.Invoke;
            }
        }

        private void AnimateMaskOut(Action onFinish)
        {
            Tweener tweenerOut = DOTween.To(OnChangeAlpha, 1, 0, maskAnimationHalfTime);
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
            maskImage.gameObject.SetActive(enable);
        }

        private void OnChangeAlpha(float value)
        {
            maskImage.color = new Color(maskImage.color.r, maskImage.color.g, maskImage.color.b, value);
        }
        
        private void OnGameQuit(QuitGameEvent evt)
        {
            AnimateMaskIn(null);
        }
    }
}