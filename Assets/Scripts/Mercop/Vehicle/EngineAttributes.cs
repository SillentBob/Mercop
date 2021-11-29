using UnityEngine;

namespace Mercop.Vehicle
{
    public abstract class EngineAttributes : ScriptableObject
    {
        public float maxPower = 2;
        public float startTime = 4;
        public float stopTime = 3;
    }
}