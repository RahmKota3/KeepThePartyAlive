using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObjects : MonoBehaviour
{
    [SerializeField] Transform pickedUpObjectParent;
    [SerializeField] PlayerStats stats;
    [SerializeField] Transform playerModel;

    Rigidbody pickedUpRigidbody = null;
    PickupableObjectType pickedUpObjectType = PickupableObjectType.None;
    List<GameObject> objectsInRange = new List<GameObject>();

    public Action<PickupableObjectType> OnPickUp;
    public Action<PickupableObjectType> OnDrop;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "PickupableObject")
        {
            objectsInRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "PickupableObject")
        {
            objectsInRange.Remove(other.gameObject);
        }
    }

    bool CanPickUpObject()
    {
        if (objectsInRange.Count == 0 || pickedUpRigidbody != null)
            return false;
        else
            return true;
    }

    bool CanThrowObject()
    {
        if (pickedUpRigidbody == null)
            return false;
        else
            return true;
    }

    void PickUpObject()
    {
        objectsInRange[objectsInRange.Count - 1].GetComponent<BoxCollider>().enabled = false;
        pickedUpRigidbody = objectsInRange[objectsInRange.Count - 1].gameObject.GetComponent<Rigidbody>();
        pickedUpRigidbody.isKinematic = true;
        pickedUpRigidbody.gameObject.transform.parent = pickedUpObjectParent;
        pickedUpRigidbody.gameObject.transform.localPosition = Vector3.zero;
        pickedUpObjectType = pickedUpRigidbody.gameObject.GetComponent<PickupableObjects>().ObjectType;

        OnPickUp?.Invoke(pickedUpObjectType);
    }

    void DropObject()
    {
        pickedUpRigidbody.gameObject.GetComponent<BoxCollider>().enabled = true;
        pickedUpRigidbody.isKinematic = false;
        pickedUpRigidbody.gameObject.transform.parent = null;
        pickedUpRigidbody.AddForce(playerModel.forward * stats.DropForce);
        pickedUpRigidbody = null;
        pickedUpObjectType = PickupableObjectType.None;

        OnDrop?.Invoke(PickupableObjectType.None);
    }

    void ThrowObject()
    {
        if (CanThrowObject() == false)
            return;

        Rigidbody previouslyPickedUpRigidbody = pickedUpRigidbody;
        DropObject();
        previouslyPickedUpRigidbody.AddForce(playerModel.forward * stats.ThrowForce);
    }

    void PickUpOrDropObject()
    {
        if (CanPickUpObject())
            PickUpObject();
        else if (pickedUpRigidbody != null)
            DropObject();
    }

    private void Awake()
    {
        InputManager.Instance.OnPickUpButtonPressed += PickUpOrDropObject;
        InputManager.Instance.OnThrowButtonPressed += ThrowObject;
    }
}
