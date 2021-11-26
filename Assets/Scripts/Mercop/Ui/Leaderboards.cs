using System;
using UnityEngine;

namespace Mercop.Ui
{
    public class Leaderboards : MonoBehaviour
    {
        public Action onEnable;

        private void OnEnable()
        {
            if (onEnable != null)
            {
                onEnable.Invoke();
            }
        }
    }
}