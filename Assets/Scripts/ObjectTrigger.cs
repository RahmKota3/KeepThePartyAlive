using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTrigger : MonoBehaviour
{
    [SerializeField] PickupableObjectType acceptedType;

    public Action<Quest> OnObjectReceived;
    Action<Quest> questFinishAction;
    Quest activeQuest;

    public void QuestCreated(Action<Quest> actionOnItemReceived, Quest quest)
    {
        OnObjectReceived += actionOnItemReceived;
        activeQuest = quest;
        acceptedType = activeQuest.TypeToGet;

        questFinishAction = actionOnItemReceived;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PickupableObject" && other.isTrigger == false)
        {
            PickupableObjects obj = other.GetComponent<PickupableObjects>();

            if (obj.ObjectType != acceptedType)
                return;

            if (obj.ObjectType != PickupableObjectType.Trash)
            {
                OnObjectReceived?.Invoke(activeQuest);
            }
            else
            {
                questFinishAction?.Invoke(obj.AssignedQuest);
            }

            Destroy(other.gameObject);
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

    private void OnDestroy()
    {
        OnObjectReceived = null;
    }
}
