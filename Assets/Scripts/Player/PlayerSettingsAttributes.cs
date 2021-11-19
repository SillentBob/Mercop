using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/Settings/PlayerSettingsAttributes")]
public class PlayerSettingsAttributes : ScriptableObject
{
    public VehicleAttribtes selectedVehicle;
    public string playerName = "Player";
    
}