using Mercop.Core;
using Mercop.Core.Events;
using UnityEngine;

namespace Mercop.Mission
{
    public class GuardianAngelContractProgressTracker : ContractProgressTracker
    {
        [SerializeField] private GameObject baseHelipad;
        [SerializeField] private GameObject survivorHelipad;

        private bool isSurvivorHelipadReached;
        private bool isReturnedToBase;

        private void Awake()
        {
            EventManager.AddListener<LandingEvent>(OnLanding);
        }

        public override bool IsCompleted()
        {
            return isSurvivorHelipadReached && isReturnedToBase;
        }

        public override void IncrementProgressStep()
        {
            EventManager.Invoke(new ContractProgressChange(this, true, IsCompleted()));
        }

        private void OnLanding(LandingEvent evt)
        {
            if (!isSurvivorHelipadReached)
            {
                if (evt.landingTarget == survivorHelipad)
                {
                    isSurvivorHelipadReached = true;
                    IncrementProgressStep();
                }
            }
            else
            {
                if (evt.landingTarget == baseHelipad)
                {
                    isReturnedToBase = true;
                    IncrementProgressStep();
                }
            }
        }
    }
}