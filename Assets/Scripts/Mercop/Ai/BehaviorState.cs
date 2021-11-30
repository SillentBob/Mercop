using UnityEngine;

namespace Mercop.Ai
{
    public abstract class BehaviorState : MonoBehaviour
    {
        protected float stateDuration;

        public virtual void OnEnter()
        {
            stateDuration = 0;
        }

        public virtual void DoUpdate()
        {
            stateDuration += Time.deltaTime;
        }

        public virtual void OnBeforeExit()
        {
        }
    }
}