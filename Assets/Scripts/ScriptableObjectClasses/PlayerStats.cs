using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "ScriptableObjects/PlayerStats", order = 0)]
public class PlayerStats : ScriptableObject
{
    public float MovementSpeed = 5;
    public float RotationSpeed = 10;
}
