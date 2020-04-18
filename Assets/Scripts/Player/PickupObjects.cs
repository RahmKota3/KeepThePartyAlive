using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObjects : MonoBehaviour
{
    [SerializeField] Transform pickedUpObjectParent;
    [SerializeField] PlayerStats stats;

    Rigidbody pickedUpRigidbody = null;
    PickupableObjectType pickedUpObjectType = PickupableObjectType.None;
    GameObject objectInRange = null;

    public Action<PickupableObjectType> OnPickUp;
    public Action<PickupableObjectType> OnDrop;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "PickupableObject")
        {
            objectInRange = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "PickupableObject")
        {
            objectInRange = null;
        }
    }

    bool CanPickUpObject()
    {
        if (objectInRange == null || pickedUpRigidbody != null)
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
        objectInRange.GetComponent<BoxCollider>().enabled = false;
        pickedUpRigidbody = objectInRange.gameObject.GetComponent<Rigidbody>();
        pickedUpRigidbody.isKinematic = true;
        objectInRange.transform.parent = pickedUpObjectParent;
        objectInRange.transform.localPosition = Vector3.zero;
        pickedUpObjectType = objectInRange.GetComponent<PickupableObjects>().ObjectType;

        OnPickUp?.Invoke(pickedUpObjectType);
    }

    void DropObject()
    {
        objectInRange.GetComponent<BoxCollider>().enabled = true;
        pickedUpRigidbody.isKinematic = false;
        objectInRange.transform.parent = null;
        pickedUpRigidbody.AddForce(transform.forward * stats.DropForce);
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
        previouslyPickedUpRigidbody.AddForce(transform.forward * stats.ThrowForce);
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
