using UnityEngine;

namespace Mercop.Mission
{
    [CreateAssetMenu(menuName = "Scriptables/Mission/MissionContractAttributes")]
    public class MissionContractAttributes : ScriptableObject
    {
        // @formatter:off
        public string contractName;
        [TextArea] public string description;
    
        [Space(20)]
        public MissionRewardAttributes reward;
        public int moneyCost;
    
        [Space(20)]
        public ReputationRequirementType requiredReputationCondition;
        public int reputationAmountRequired;
    
        [Space(20)]
        public bool completed;
        public string sceneName;

        // @formatter:on

        public enum ReputationRequirementType
        {
            NONE,
            EQUAL_OR_LESS_THAN,
            EQUAL_OR_MORE_THAN
        }
    }
}