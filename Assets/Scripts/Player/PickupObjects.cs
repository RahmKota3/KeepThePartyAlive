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
        if (CanPickUpObject() == false)
            return;

        objectInRange.GetComponent<BoxCollider>().enabled = false;
        pickedUpRigidbody = objectInRange.gameObject.GetComponent<Rigidbody>();
        pickedUpRigidbody.isKinematic = true;
        objectInRange.transform.parent = pickedUpObjectParent;
        objectInRange.transform.localPosition = Vector3.zero;
    }

    private void Awake()
    {
        InputManager.Instance.OnPickUpButtonPressed += PickUpObject;
    }
}
