using System;
using Mercop.Core;
using Mercop.Core.Events;
using Mercop.Vehicle;
using UnityEngine;

namespace Mercop.Player
{
    public class PlayerController : MonoBehaviour
    {
        // @formatter:off
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private EngineController engineController;
        // @formatter:on

        private VehicleAttribtes vehicleAttribtes;

        private void Awake()
        {
            vehicleAttribtes = GameManager.Instance.playerSettings.selectedVehicle;
            rigidbody.maxAngularVelocity = vehicleAttribtes.rotationMaxRadiansPerSec;
            engineController.VehicleAttribtes = vehicleAttribtes;
        }

        public void Rotate(Vector3 input)
        {
            if (engineController.IsFullyOperational())
            {
                rigidbody.AddRelativeTorque(0,
                    input.y * vehicleAttribtes.rotationAcceleration, 0,
                    vehicleAttribtes.rotationForceMode);
            }
        }

        public void Move(Vector3 input)
        {
            if (engineController.IsFullyOperational())
            {
                if (rigidbody.velocity.magnitude < vehicleAttribtes.moveMaxSpeed)
                {
                    Vector3 forwardSpeed =
                        new Vector3(0, 0, input.y * vehicleAttribtes.moveAcceleration);
                    //Debug.Log(forwardSpeed);
                    rigidbody.AddRelativeForce(forwardSpeed);
                }
            }
        }

        public void StartEngine()
        {
            engineController.StartEngine();
        }

        public void StopEngine()
        {
            engineController.StopEngine();
        }
    }
}