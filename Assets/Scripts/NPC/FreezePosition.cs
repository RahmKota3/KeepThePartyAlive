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

        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    IEnumerator FreezePosCoroutine()
    {
        yield return new WaitForSeconds(1f);

        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
}
