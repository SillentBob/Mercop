using System;
using System.Collections.Generic;
using Mercop.Ui;
using UnityEngine;
using UnityEngine.Serialization;

namespace Mercop.Core
{
    public class StatesManager : Singleton<StatesManager>
    {
        [FormerlySerializedAs("views")] 
        [SerializeField] private List<State> states;
        private State previousState;
        private State currentState;

        public void PreviousState()
        {
            if (previousState != null)
            {
                AnimateSwitchToState(previousState);
            }
        }

        public void LoadState<T>(Action onShowAnimationFinish = null) where T : State
        {
            Debug.Log($"LoadState {typeof(T)}");
            foreach (var view in states)
            {
                if (view.GetType() == typeof(T))
                {
                    AnimateSwitchToState(view, onShowAnimationFinish);
                    return;
                }
            }
        }

        private void AnimateSwitchToState(State state, Action onAnimationFinish = null)
        {
            if (state != currentState)
            {
                var v = state;
                ScreenMaskManager.Instance.MaybeAnimateShowMask(() =>
                {
                    DoSwitchState(v);
                    if (onAnimationFinish != null)
                    {
                        onAnimationFinish.Invoke();
                    }
                });
            }
        }

        private void DoSwitchState(State state)
        {
            previousState = currentState;
            if (previousState != null)
            {
                previousState.OnStateExit();
            }
            currentState = state;
            currentState.OnStateEnter();
            Debug.Log($"ShowView finished");
        }
    }
}