using UnityEngine;

namespace Mercop.Player
{
    [CreateAssetMenu(menuName = "Scriptables/Player/PlayerResourcesAttributes")]
    public class PlayerResourcesAttributes : ScriptableObject
    {
        public int money;
        public int reputation;
        public int experience;
    }
}