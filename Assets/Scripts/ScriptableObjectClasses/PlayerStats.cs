using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "ScriptableObjects/PlayerStats", order = 0)]
public class PlayerStats : ScriptableObject
{
    [Header("Movement")]
    public float MovementSpeed = 5;
    public float RotationSpeed = 10;

    [Header("Throwing")]
    public float DropForce = 100;
    public float ThrowForce = 750;
}
