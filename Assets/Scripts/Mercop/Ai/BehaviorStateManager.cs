using UnityEngine;

namespace Mercop.Ai
{
    public abstract class BehaviorStateManager : MonoBehaviour
    {
        private BehaviorState lastState;
        private BehaviorState currentState;

        protected virtual void Update()
        {
            if (GetCurrentState() != null)
            {
                GetCurrentState().DoUpdate();
            }
        }

        protected virtual void SetCurrentState(BehaviorState state)
        {
            if (currentState != null)
            {
                currentState.OnBeforeExit();
            }

            lastState = currentState;
            currentState = state;
            if (currentState != null)
            {
                currentState.OnEnter();
            }
        }

        protected BehaviorState GetCurrentState()
        {
            return currentState;
        }

        protected BehaviorState GetLastState()
        {
            return lastState;
        }

        public virtual void OnStateChange(BehaviorState lastState, BehaviorState newState)
        {
        }

        public virtual void RequestState(BehaviorState state)
        {
            SetCurrentState(state);
        }
    }
}