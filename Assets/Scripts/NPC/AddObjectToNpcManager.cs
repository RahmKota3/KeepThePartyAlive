using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddObjectToNpcManager : MonoBehaviour
{
    private void Start()
    {
        NPCManager.Instance.AddNpcToList(this.gameObject);
    }
}
