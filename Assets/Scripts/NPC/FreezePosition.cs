using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezePosition : MonoBehaviour
{
    Rigidbody rb;

    public void FreezePos()
    {
        StopAllCoroutines();
        StartCoroutine(FreezePosCoroutine());
    }
    public void UnFreezePos()
    {
        StopAllCoroutines();

        rb.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
    }

    IEnumerator FreezePosCoroutine()
    {
        yield return new WaitForSeconds(1f);

        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
}
