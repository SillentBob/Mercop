using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ContractProgressTracker : MonoBehaviour
{
    public virtual bool IsCompleted()
    {
        return false;
    }

    public virtual void IncrementProgressStep()
    {
        
    }

}
