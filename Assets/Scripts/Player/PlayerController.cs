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

    //private void FixedUpdate()
    //{
        // AccelerateOrDeccelerate(targetForwardSpeed);
        //rigidbody.velocity = rigidbody.transform.forward * currentForwardSpeed;
    //}

    // private void AccelerateTo(float target)
    // {
    //     if (currentForwardSpeed < target)
    //     {
    //         currentForwardSpeed += Time.fixedDeltaTime *
    //                                GameManager.Instance.playerSettings.selectedVehicle.acceleration;
    //         if (currentForwardSpeed > target)
    //         {
    //             currentForwardSpeed = target;
    //         }
    //     }
    // }
    //
    // private void DeccelerateTo(float target)
    // {
    //     currentForwardSpeed -= Time.fixedDeltaTime *
    //                            GameManager.Instance.playerSettings.selectedVehicle.acceleration ;
    //     if (currentForwardSpeed < target)
    //     {
    //         currentForwardSpeed = target;
    //     }
    // }
    //
    // private void AccelerateOrDeccelerate(float targetValue)
    // {
    //     if (targetValue < currentForwardSpeed)
    //     {
    //         DeccelerateTo(targetValue);
    //     }
    //     else if (targetValue > currentForwardSpeed)
    //     {
    //         AccelerateTo(targetValue);
    //     }
    // }
    
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
        //targetForwardSpeed = GameManager.Instance.playerSettings.selectedVehicle.maxSpeed * input.y;
    }
}