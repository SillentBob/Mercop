using UnityEngine;

namespace Mercop.Core
{
    public abstract class View : MonoBehaviour
    {
        public abstract void OnShow();
        public abstract void OnHide();
    }
}