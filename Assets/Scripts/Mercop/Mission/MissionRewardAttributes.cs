using UnityEngine;

namespace Mercop.Mission
{
    [CreateAssetMenu(menuName = "Scriptables/Mission/MissionRewardAttributes")]
    public class MissionRewardAttributes : ScriptableObject
    {
        public int money;
        public int reputation;
        public int experience;
    }
}
