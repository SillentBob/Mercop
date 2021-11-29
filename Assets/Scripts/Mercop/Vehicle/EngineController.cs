using UnityEngine;

namespace Mercop.Vehicle
{
    public abstract class EngineController : MonoBehaviour
    {
        public EngineAttributes Engine { protected get; set; }
        public abstract bool IsFullyOperational();
        public abstract void StartEngine();
        public abstract void StopEngine();
    }
}