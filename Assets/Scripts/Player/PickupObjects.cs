using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObjects : MonoBehaviour
{
    [SerializeField] Transform pickedUpObjectParent;

    Rigidbody pickedUpRigidbody = null;
    GameObject objectInRange = null;

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

    void PickUpObject()
    {
        objectInRange.GetComponent<BoxCollider>().enabled = false;
        pickedUpRigidbody = objectInRange.gameObject.GetComponent<Rigidbody>();
        pickedUpRigidbody.isKinematic = true;
        objectInRange.transform.parent = pickedUpObjectParent;
        objectInRange.transform.localPosition = Vector3.zero;
    }

    void DropObject()
    {
        objectInRange.GetComponent<BoxCollider>().enabled = true;
        pickedUpRigidbody.isKinematic = false;
        objectInRange.transform.parent = null;
        pickedUpRigidbody = null;
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
    }
}
