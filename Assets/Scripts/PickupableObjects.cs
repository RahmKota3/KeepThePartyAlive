using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickupableObjectType { None, Beer, Soda, Chips, Trash, Trashbag, Ball }

public class PickupableObjects : MonoBehaviour
{
    public PickupableObjectType ObjectType;

    private void Awake()
    {
        if(ObjectType == PickupableObjectType.None)
        {
            Debug.LogError("PickupableObjectType.None shouldn't appear in game: " + gameObject.name);
        }
    }
}
