using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform rootTransform;
    [SerializeField] private Rigidbody rigidbody;

    private bool isInputForward;

    private float targetForwardSpeed;
    private float currentForwardSpeed;

    private void Awake()
    {
        rigidbody.maxAngularVelocity = 1;
    }

    public void Rotate(Vector3 input)
    {
        rigidbody.AddRelativeTorque(0, input.y * GameManager.Instance.playerSettings.selectedVehicle.rotateSpeed, 0,
            ForceMode.Impulse);
    }

    public void Move(Vector3 input)
    {
        if (rigidbody.velocity.magnitude < GameManager.Instance.playerSettings.selectedVehicle.maxSpeed)
        {
            Vector3 fowardSpeed =
                new Vector3(0, 0, input.y * GameManager.Instance.playerSettings.selectedVehicle.acceleration);
            rigidbody.AddRelativeForce(fowardSpeed, ForceMode.Impulse);
        }
    }
}