using UnityEngine;
using Vehicle;

namespace Player
{
    [CreateAssetMenu(menuName = "Scriptables/Settings/PlayerSettingsAttributes")]
    public class PlayerSettingsAttributes : ScriptableObject
    {
        public VehicleAttribtes selectedVehicle;
        public string playerName = "Player";
    
    }
}