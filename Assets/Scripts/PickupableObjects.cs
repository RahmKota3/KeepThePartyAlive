using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickupableObjectType { None, Beer, Soda, Chips, Trash, Trashbag }

public class PickupableObjects : MonoBehaviour
{
    public PickupableObjectType ObjectType;
}
