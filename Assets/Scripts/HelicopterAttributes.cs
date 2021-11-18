using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/Vehicles/HelicopterAttributes")]
public class HelicopterAttributes : VehicleAttribtes
{
    [Range(1, 100)] public float strafeSpeed = 1;
}