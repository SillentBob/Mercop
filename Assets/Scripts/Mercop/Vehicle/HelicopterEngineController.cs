using DG.Tweening;
using Mercop.Core;
using Mercop.Core.Events;
using UnityEngine;

namespace Mercop.Vehicle
{
    public class HelicopterEngineController : EngineController
    {
        private EngineState currentState;
        [SerializeField] private Transform mainRotor;
        [SerializeField] private Transform backRotor;
        private float currentEnginePower;

        private Animator mainRotorAnimator;

        private void Awake()
        {
            currentState = EngineState.Stopped;
            mainRotorAnimator = mainRotor.gameObject.GetComponent<Animator>();
            mainRotorAnimator.speed = 0;
        }

        public override void StartEngine()
        {
            if (currentState == EngineState.Stopped)
            {
                currentState = EngineState.Starting;
                EventManager.Invoke(new EngineEvent(EngineEvent.EngineEventType.StartBegin));
                Tweener engineStartTween = DOTween.To(OnEnginePowerIncrease, 0, Engine.maxPower, Engine.startTime);
                engineStartTween.onComplete += OnEngineStartFinish;
            }
        }
        
        private void OnEngineStartFinish()
        {
            currentState = EngineState.Started;
            EventManager.Invoke(new EngineEvent(EngineEvent.EngineEventType.StartFinished));
        }
        
        public override void StopEngine()
        {
            if (currentState == EngineState.Started)
            {
                currentState = EngineState.Stopping;
                EventManager.Invoke(new EngineEvent(EngineEvent.EngineEventType.StopBegin));
                Tweener engineStopTween = DOTween.To(OnEnginePowerDecrease, Engine.maxPower, 0, Engine.stopTime);
                engineStopTween.onComplete += OnEngineStopFinish;
            }
        }
        
        private void OnEngineStopFinish()
        {
            currentState = EngineState.Stopped;
            EventManager.Invoke(new EngineEvent(EngineEvent.EngineEventType.StopFinish));
        }

        private enum EngineState
        {
            Starting,Started,Stopping,Stopped
        }

        private void OnEnginePowerIncrease(float power)
        {
            currentEnginePower = power;
            mainRotorAnimator.speed = currentEnginePower;
        }
        
        private void OnEnginePowerDecrease(float power)
        {
            currentEnginePower = power;
            mainRotorAnimator.speed = currentEnginePower;
        }

        public override bool IsFullyOperational()
        {
            return currentState == EngineState.Started;
        }
        
    }
}