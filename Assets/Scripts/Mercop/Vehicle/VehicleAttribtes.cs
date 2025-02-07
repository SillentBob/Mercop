using System;
using UnityEngine;

namespace Mercop.Vehicle
{
    [Serializable]
    public abstract class VehicleAttribtes : ScriptableObject
    {
        [Range(1, 1000)] public float moveMaxSpeed = 10;

        [Range(1, 10000)] public float moveAcceleration = 1000;

        public ForceMode moveForceMode = ForceMode.Impulse;

        [Range(1, 500)] public float rotationAcceleration = 1;
        public float rotationMaxRadiansPerSec = 2;
        public ForceMode rotationForceMode = ForceMode.Impulse;

        [Range(1, 1000)] public float maxHealth = 100;
        [Range(1, 1000)] public float maxFuel = 500;

        public EngineAttributes engineAttributes;
    }
}