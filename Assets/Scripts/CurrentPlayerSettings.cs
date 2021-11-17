using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/Settings/CurrentPlayerSettings")]
public class CurrentPlayerSettings : ScriptableObject
{
    public VehicleAttribtes selectedVehicle;
    public string playerName = "Player";
}