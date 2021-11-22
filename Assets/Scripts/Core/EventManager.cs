using System;
using System.Collections.Generic;
using Core.Events;
using UnityEngine;

namespace Core
{
    public class EventManager : MonoBehaviour
    {
        private static Dictionary<PauseEventType, Action<PauseEvent>> pauseEvents =
            new Dictionary<PauseEventType, Action<PauseEvent>>();

        public static void AddListener(PauseEventType eventType, Action<PauseEvent> onEventAction)
        {
            if (!pauseEvents.ContainsKey(eventType))
            {
                pauseEvents.Add(eventType, onEventAction);
            }
            else
            {
                pauseEvents[eventType] += onEventAction;
            }
        }

        public static void Invoke(PauseEventType eventType, PauseEvent evt)
        {
            if (pauseEvents.ContainsKey(eventType))
            {
                pauseEvents[eventType].Invoke(evt);
            }
        }
    }

    public static class EventTypes
    {
        public static readonly PauseEventType Pause = new PauseEventType();
    }
    
}