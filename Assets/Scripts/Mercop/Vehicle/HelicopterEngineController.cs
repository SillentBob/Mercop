using System;
using DG.Tweening;
using Mercop.Audio;
using Mercop.Core;
using Mercop.Core.Events;
using UnityEngine;

namespace Mercop.Vehicle
{
    public class HelicopterEngineController : EngineController
    {
        [SerializeField] private Transform mainRotor;
        [SerializeField] private Transform backRotor;

        private float currentEnginePowerPercentage;
        private Animator mainRotorAnimator;
        private EngineState currentEngineState;
        private Vector3 objectStartPosition;

        private HelicopterAttributes helicopterVehicleAttribtes;
        private HelicopterEngineAttributes helicopterEngineAttribtes;
        
        private void Start()
        {
            helicopterVehicleAttribtes = (HelicopterAttributes)VehicleAttribtes;
            helicopterEngineAttribtes = helicopterVehicleAttribtes.engineAttributes as HelicopterEngineAttributes;

            objectStartPosition = movableObjectRootTransform.position;
            currentEngineState = EngineState.Stopped;
            mainRotorAnimator = mainRotor.gameObject.GetComponent<Animator>();
            mainRotorAnimator.speed = 0;
        }

        public override void StartEngine()
        {
            if (currentEngineState == EngineState.Stopped)
            {
                currentEngineState = EngineState.Starting;
                EventManager.Invoke(new EngineEvent(EngineEvent.EngineEventType.StartBegin));
                Tweener engineStartTween = DOTween.To(OnEnginePowerIncrease, 0, 1,
                    helicopterEngineAttribtes.startTime);
                engineStartTween.onComplete += OnEngineStartFinish;
                AudioPlayer.Instance.Play(AudioPlayer.Sound.Engine);
            }
        }

        private void OnEngineStartFinish()
        {
            currentEngineState = EngineState.Started;
            EventManager.Invoke(new EngineEvent(EngineEvent.EngineEventType.StartFinish));
        }

        public override void StopEngine()
        {
            if (currentEngineState == EngineState.Started)
            {
                currentEngineState = EngineState.Stopping;
                EventManager.Invoke(new EngineEvent(EngineEvent.EngineEventType.StopBegin));
                Tweener engineStopTween = DOTween.To(OnEnginePowerDecrease, 1,
                    0, VehicleAttribtes.engineAttributes.stopTime);
                engineStopTween.onComplete += OnEngineStopFinish;
            }
        }

        private void OnEngineStopFinish()
        {
            currentEngineState = EngineState.Stopped;
            EventManager.Invoke(new EngineEvent(EngineEvent.EngineEventType.StopFinish));
            AudioPlayer.Instance.Play(AudioPlayer.Sound.Engine);
        }


        private void OnEnginePowerIncrease(float power)
        {
            currentEnginePowerPercentage = power;
            mainRotorAnimator.speed = currentEnginePowerPercentage * helicopterEngineAttribtes.maxPower;

            var liftDelta = Mathf.Lerp(0, 1, (currentEnginePowerPercentage - 0.5f) * 2);
            LiftObject(liftDelta);
            
            AudioPlayer.Instance.ChangePitch(AudioPlayer.Sound.Engine, currentEnginePowerPercentage);
        }

        private void OnEnginePowerDecrease(float power)
        {
            currentEnginePowerPercentage = power;
            mainRotorAnimator.speed = currentEnginePowerPercentage * helicopterEngineAttribtes.maxPower;

            var liftDelta = Mathf.Lerp(0, 1, (currentEnginePowerPercentage - 0.5f) * 2);
            LiftObject(liftDelta);
            
            AudioPlayer.Instance.ChangePitch(AudioPlayer.Sound.Engine, currentEnginePowerPercentage);
        }

        private void LiftObject(float heightDelta)
        {
            var position = movableObjectRootTransform.position;
            position =
                new Vector3(position.x, heightDelta * helicopterVehicleAttribtes.maxLiftHeight, position.z);
            movableObjectRootTransform.position = position;
        }

        public override bool IsFullyOperational()
        {
            return currentEngineState == EngineState.Started;
        }

        private enum EngineState
        {
            Starting,
            Started,
            Stopping,
            Stopped
        }
    }
}