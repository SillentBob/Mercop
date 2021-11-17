using UnityEngine;

public abstract class VehicleAttribtes : ScriptableObject
{
    [Range(1, 100)] public float moveSpeed = 10;

    [Range(1, 500)] public float rotateSpeed = 1;

    [Range(1, 1000)] public float maxHealth = 100;

    [Range(1, 1000)] public float maxFuel = 500;
}