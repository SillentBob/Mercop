using System;
using System.Collections.Generic;
using Mercop.Ui;
using UnityEngine;

namespace Mercop.Core
{
    public class ViewManager : Singleton<ViewManager>
    {
        [SerializeField] private List<View> views;
        private View previousView;
        private View currentView;

        public void PreviousView()
        {
            if (previousView != null)
            {
                AnimateSwitchToView(previousView);
            }
        }

        public void ShowView<T>(Action onShowAnimationFinish = null) where T : View
        {
            Debug.Log($"ShowView {typeof(T)}");
            foreach (var view in views)
            {
                if (view.GetType() == typeof(T))
                {
                    AnimateSwitchToView(view, onShowAnimationFinish);
                    return;
                }
            }
        }

        private void AnimateSwitchToView(View view, Action onAnimationFinish = null)
        {
            if (view != currentView)
            {
                var v = view;
                ScreenMaskManager.Instance.MaybeAnimateShowMask(() =>
                {
                    DoSwitchView(v);
                    if (onAnimationFinish != null)
                    {
                        onAnimationFinish.Invoke();
                    }
                });
            }
        }

        private void DoSwitchView(View view)
        {
            previousView = currentView;
            if (previousView != null)
            {
                previousView.OnHide();
            }
            currentView = view;
            currentView.OnShow();
            Debug.Log($"ShowView finished");
        }
    }
}