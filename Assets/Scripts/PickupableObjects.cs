using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickupableObjectType { None, Beer, Soda, Chips, Trash, Trashbag, Ball, Guest }

public class PickupableObjects : MonoBehaviour
{
    public PickupableObjectType ObjectType;

    private void OnDestroy()
    {
        PickupObjects pickup = FindObjectOfType<PickupObjects>();
        
        if(pickup != null)
            pickup.ObjDestroyed(this.gameObject);
    }

    private void Awake()
    {
        if(ObjectType == PickupableObjectType.None)
        {
            Debug.LogError("PickupableObjectType.None shouldn't appear in game: " + gameObject.name);
        }
    }
}
