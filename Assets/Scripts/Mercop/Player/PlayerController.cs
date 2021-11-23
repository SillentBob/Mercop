using Mercop.Core;
using UnityEngine;

namespace Mercop.Player
{
    //TODO save selectedVehicle values here and read in rotate and move for FPS optimisations later.(rotate,move)
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Transform rootTransform;
        [SerializeField] private new Rigidbody rigidbody;

        private void Awake()
        {
            rigidbody.maxAngularVelocity = GameManager.Instance.playerSettings.selectedVehicle.rotationMaxRadiansPerSec;
        }

        public void Rotate(Vector3 input)
        {
            rigidbody.AddRelativeTorque(0,
                input.y * GameManager.Instance.playerSettings.selectedVehicle.rotationAcceleration, 0,
                GameManager.Instance.playerSettings.selectedVehicle.rotationForceMode);
        }

        public void Move(Vector3 input)
        {
            if (rigidbody.velocity.magnitude < GameManager.Instance.playerSettings.selectedVehicle.moveMaxSpeed)
            {
                Vector3 forwardSpeed =
                    new Vector3(0, 0, input.y * GameManager.Instance.playerSettings.selectedVehicle.moveAcceleration);
                rigidbody.AddRelativeForce(forwardSpeed,
                    GameManager.Instance.playerSettings.selectedVehicle.moveForceMode);
            }
        }
    }
}