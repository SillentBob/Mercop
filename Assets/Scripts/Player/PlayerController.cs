using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform rootTransform;
    
    public void Rotate(Vector3 input)
    {
        rootTransform.Rotate(0, input.y * GameManager.Instance.playerSettings.selectedVehicle.rotateSpeed , 0);
    }

    public void Move(Vector3 input)
    {
        rootTransform.localPosition += rootTransform.forward * input.y * GameManager.Instance.playerSettings.selectedVehicle.moveSpeed;
    }

}