using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTrigger : MonoBehaviour
{
    [SerializeField] PickupableObjectType acceptedType;

    public Action<Quest> OnObjectReceived;
    Quest activeQuest;

    public void QuestCreated(Action<Quest> actionOnItemReceived, Quest quest)
    {
        OnObjectReceived += actionOnItemReceived;
        activeQuest = quest;
        acceptedType = activeQuest.TypeToGet;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PickupableObject" && other.isTrigger == false && 
            other.GetComponent<PickupableObjects>().ObjectType == acceptedType)
        {
            Destroy(other.gameObject);
            OnObjectReceived?.Invoke(activeQuest);
            //OnObjectReceived = null;
        }
    }

    void ResetAction()
    {
        OnObjectReceived = null;
        LevelManager.Instance.OnBeforeSceneLoad -= ResetAction;
    }

    private void Awake()
    {
        LevelManager.Instance.OnBeforeSceneLoad += ResetAction;
    }
}
