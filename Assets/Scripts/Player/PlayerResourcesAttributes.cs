using UnityEngine;

namespace Player
{
    [CreateAssetMenu(menuName = "Scripatbles/Player/PlayerResourcesAttributes")]
    public class PlayerResourcesAttributes : ScriptableObject
    {
        public int money;
        public int reputation;
        public int experience;
    }
}