using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceManager : MonoBehaviour
{
    public static ReferenceManager Instance;

    public bool StaticCamera = false;

    private void Awake()
    {
        Instance = this;
    }
}
