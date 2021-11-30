using System.Collections.Generic;
using Mercop.Core;
using Mercop.Core.Events;
using UnityEngine;
using UnityEngine.Serialization;

public class ContractListTracker : MonoBehaviour
{
    [FormerlySerializedAs("quests")] [SerializeField]
    private List<ContractProgressTracker> trackers;

    private void Awake()
    {
        EventManager.AddListener<ContractProgressChange>(OnContractProgressChange);
    }

    private void OnContractProgressChange(ContractProgressChange evt)
    {
        if (IsAllQuestsCompleted())
        {
            EventManager.Invoke(new ContractsFinishEvent());
        }
    }

    private bool IsAllQuestsCompleted()
    {
        if (trackers != null)
        {
            foreach (ContractProgressTracker tracker in trackers)
            {
                if (!tracker.IsCompleted())
                {
                    return false;
                }
            }
        }
        else
        {
            return false;
        }

        return true;
    }
}