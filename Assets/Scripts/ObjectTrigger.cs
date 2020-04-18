using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PickupableObject" && other.isTrigger == false)
        {
            Destroy(other.gameObject);
            // TODO: Send info to QuestManager about collected object.
        }
    }
}
