using UnityEngine;

namespace Mercop.Core.Events
{
    public class LandingEvent : BaseEvent
    {
        public GameObject landingTarget;
        
        public LandingEvent(GameObject landingTarget)
        {
            this.landingTarget = landingTarget;
        }
    }
}