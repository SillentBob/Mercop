using UnityEngine;

namespace Mercop.Vehicle
{
    public abstract class EngineController : MonoBehaviour
    {
        public Transform movableObjectRootTransform;
        public VehicleAttribtes VehicleAttribtes { protected get; set; }
        public abstract bool IsFullyOperational();
        public abstract void StartEngine();
        public abstract void StopEngine();
    }
}