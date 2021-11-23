using System;
using System.Collections.Generic;
using Mercop.Core.Events;
using UnityEngine;

namespace Mercop.Core
{
    public class EventManager : MonoBehaviour
    {
        private static Dictionary<PauseEventType, Action<PauseEvent>> pauseEvents =
            new Dictionary<PauseEventType, Action<PauseEvent>>();
        private static Dictionary<QuitGameEventType, Action<QuitGameEvent>> quitEvents =
            new Dictionary<QuitGameEventType, Action<QuitGameEvent>>();
        private static Dictionary<LoadSceneEventType, Action<LoadSceneEvent>> loadSceneEvents =
            new Dictionary<LoadSceneEventType, Action<LoadSceneEvent>>();
        
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
        
        public static void AddListener(QuitGameEventType eventType, Action<QuitGameEvent> onEventAction)
        {
            if (!quitEvents.ContainsKey(eventType))
            {
                quitEvents.Add(eventType, onEventAction);
            }
            else
            {
                quitEvents[eventType] += onEventAction;
            }
        }

        public static void Invoke(QuitGameEventType eventType, QuitGameEvent evt)
        {
            if (quitEvents.ContainsKey(eventType))
            {
                quitEvents[eventType].Invoke(evt);
            }
        }
        
        public static void AddListener(LoadSceneEventType eventType, Action<LoadSceneEvent> onEventAction)
        {
            if (!loadSceneEvents.ContainsKey(eventType))
            {
                loadSceneEvents.Add(eventType, onEventAction);
            }
            else
            {
                loadSceneEvents[eventType] += onEventAction;
            }
        }

        public static void Invoke(LoadSceneEventType eventType, LoadSceneEvent evt)
        {
            if (loadSceneEvents.ContainsKey(eventType))
            {
                loadSceneEvents[eventType].Invoke(evt);
            }
        }
    }

    public static class EventTypes
    {
        public static readonly PauseEventType Pause = new PauseEventType();
        public static readonly QuitGameEventType Quit = new QuitGameEventType();
        public static readonly LoadSceneEventType LoadScene = new LoadSceneEventType();
    }
    
}