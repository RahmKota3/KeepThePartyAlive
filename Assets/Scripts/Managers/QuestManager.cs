﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [Header("Timers")]
    [SerializeField] float timeBetweenQuests = 15;
    [SerializeField] float timeBeforeQuestFail = 15;
    [SerializeField] float timeBetweenQuestsReduction = 0;
    [SerializeField] float timeBeforeQuestFailReduction = 0;
    [SerializeField] float minTimeBetweenQuests = 10;
    [SerializeField] float minTimeBeforeQuestFails = 10;

    [SerializeField] float pukeRange = 3f;

    [Header("References")]
    [SerializeField] GameObject itemReceiverPrefab;
    [SerializeField] GameObject questMarkerPrefab;
    [SerializeField] GameObject trashPrefab;
    [SerializeField] ObjectTrigger trashcan;
    [SerializeField] GameObject ui;
    [SerializeField] GameObject pukePrefab;

    PickupObjects playerPickupObjs;

    List<Quest> activeQuests = new List<Quest>();
    Dictionary<Quest, List<GameObject>> objectsToDestroyOnQuestFinish = new Dictionary<Quest, List<GameObject>>();

    Quest GetRandomizedQuest()
    {
        Transform randomNpc = NPCManager.Instance.NPCs[Random.Range(0, NPCManager.Instance.NPCs.Count)].transform;

        QuestType typeOfQuest = (QuestType)RandomExtension.ChooseFromMultipleWeighted(new List<int> { (int)QuestType.GetSomething,
            (int)QuestType.ChangeMusic, (int)QuestType.ThrowTheTrashOut, (int)QuestType.Puking }, new List<int> { 60, 10, 30, 20 });

        //QuestType typeOfQuest = QuestType.Puking;

        QuestState npcActiveQuest = randomNpc.gameObject.GetComponent<QuestState>();
        if (npcActiveQuest.ActiveQuest != null) 
        {
            int i = 0;

            while (npcActiveQuest.ActiveQuest.TypeOfQuest == QuestType.Puking)
            {
                randomNpc = NPCManager.Instance.NPCs[Random.Range(0, NPCManager.Instance.NPCs.Count)].transform;
                i++;

                if (i == 100)
                {
                    typeOfQuest = QuestType.GetSomething;
                    break;
                }
            }
        }

        PickupableObjectType objType = PickupableObjectType.None;

        if(typeOfQuest == QuestType.GetSomething)
        {
            // Debug
            objType = PickupableObjectType.Beer;
        }
        else if(typeOfQuest == QuestType.ThrowTheTrashOut)
        {
            objType = PickupableObjectType.Trash;
        }

        return new Quest(typeOfQuest, timeBeforeQuestFail, objType, randomNpc);
    }

    void CreateNewQuest()
    {
        Coroutine c;
        Quest quest = GetRandomizedQuest();
        activeQuests.Add(quest);

        TimeManager.Instance.StartQuestFailTimer(timeBeforeQuestFail, FailQuest, quest, out c);
        TimeManager.Instance.StartTimer(timeBetweenQuests, CreateNewQuest);

        quest.FailCoroutine = c;

        ChangeQuestTimers();
        CreateObjectsNeededForQuest(quest);

        if(quest.TypeOfQuest == QuestType.ThrowTheTrashOut)
        {
            trashcan.QuestCreated(FinishQuest, quest);
        }

        if(quest.TypeOfQuest != QuestType.Puking)
        {
            quest.Npc.GetComponent<StateMachine>().ChangeState(NpcState.Waiting);
        }
        else
        {
            quest.Npc.GetComponent<StateMachine>().ChangeState(NpcState.Sick);
        }

        quest.Npc.GetComponent<QuestState>().ActiveQuest = quest;

        Debug.Log("New quest: " + quest.TypeOfQuest);
    }

    void CreateObjectsNeededForQuest(Quest quest)
    {
        GameObject objToDestroyLater;

        objectsToDestroyOnQuestFinish[quest] = new List<GameObject>();

        if(quest.TypeOfQuest == QuestType.GetSomething)
        {
            objToDestroyLater = Instantiate(itemReceiverPrefab, quest.Npc.position, Quaternion.identity, quest.Npc);
            objToDestroyLater.GetComponent<ObjectTrigger>().QuestCreated(FinishQuest, quest);
            objectsToDestroyOnQuestFinish[quest].Add(objToDestroyLater);
        }
        else if(quest.TypeOfQuest == QuestType.ThrowTheTrashOut)
        {
            objToDestroyLater = Instantiate(trashPrefab, quest.Npc.Find("TrashSpawnPosition").position, Quaternion.identity);
            objectsToDestroyOnQuestFinish[quest].Add(objToDestroyLater);
        }

        objToDestroyLater = Instantiate(questMarkerPrefab, ui.transform);
        objToDestroyLater.GetComponent<PointTowardsQuest>().questWorldPosition = quest.Npc.position + new Vector3(0, 2.5f, 0);
        objectsToDestroyOnQuestFinish[quest].Add(objToDestroyLater);
    }

    void FinishQuest(Quest finishedQuest)
    {
        Debug.Log("Finished quest: " + finishedQuest.TypeOfQuest);

        finishedQuest.Npc.GetComponent<QuestState>().ActiveQuest.TypeOfQuest = QuestType.None;

        finishedQuest.Npc.GetComponent<StateMachine>().ChangeState(NpcState.Happy);

        TimeManager.Instance.StopCoroutine(finishedQuest.FailCoroutine);

        DestroyQuestObjects(finishedQuest);

        activeQuests.Remove(finishedQuest);
        // TODO: Add score.
    }

    void FailQuest(Quest questToFail)
    {
        Debug.Log("Failed quest: " + questToFail.TypeOfQuest);

        CreateQuestFailObjects(questToFail);

        DestroyQuestObjects(questToFail);

        activeQuests.Remove(questToFail);
        // TODO: Remove score.

        if (questToFail.Npc != null)
        {
            if(playerPickupObjs.pickedUpRigidbody != null && playerPickupObjs.pickedUpRigidbody.gameObject.transform == questToFail.Npc)
            {
                playerPickupObjs.DropObject();
            }
            ChangeNpcsState(questToFail);
            questToFail.Npc.GetComponent<QuestState>().ActiveQuest.TypeOfQuest = QuestType.None;
        }
    }

    void CreateQuestFailObjects(Quest quest)
    {
        if(quest.TypeOfQuest == QuestType.Puking)
        {
            Instantiate(pukePrefab, quest.Npc.Find("TrashSpawnPosition").position, Quaternion.Euler(0, Random.Range(0f, 359f), 0));
        }
    }

    void DestroyQuestObjects(Quest quest)
    {
        if (objectsToDestroyOnQuestFinish.ContainsKey(quest))
        {
            foreach (GameObject g in objectsToDestroyOnQuestFinish[quest])
            {
                Destroy(g);
            }
        }
    }

    void ChangeNpcsState(Quest quest)
    {
        if(quest.TypeOfQuest != QuestType.Puking)
        {
            quest.Npc.GetComponent<StateMachine>().ChangeState(NpcState.Angry);
        }
        else
        {
            quest.Npc.GetComponent<StateMachine>().ChangeState(NpcState.Happy);

            foreach (GameObject g in NPCManager.Instance.NPCs)
            {
                if(Vector3.Distance(quest.Npc.position, g.transform.position) <= pukeRange && g.transform != quest.Npc)
                {
                    g.GetComponent<StateMachine>().ChangeState(NpcState.Angry);
                }
            }
        }
    }

    void ChangeQuestTimers()
    {
        timeBeforeQuestFail = Mathf.Clamp(timeBeforeQuestFail - timeBeforeQuestFailReduction, minTimeBeforeQuestFails, timeBeforeQuestFail);
        timeBetweenQuests = Mathf.Clamp(timeBetweenQuests - timeBeforeQuestFailReduction, minTimeBetweenQuests, timeBetweenQuests);
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        playerPickupObjs = FindObjectOfType<PickupObjects>();

        //TimeManager.Instance.StartTimer(2, CreateNewQuest);
        TimeManager.Instance.StartTimer(0, CreateNewQuest);
    }
}
