using UnityEngine;

namespace Mercop.Core
{
    public abstract class State : MonoBehaviour
    {
        public abstract void OnStateEnter();
        public abstract void OnStateExit();
    }
}