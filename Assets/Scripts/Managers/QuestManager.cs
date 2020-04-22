using System.Collections;
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
    [SerializeField] Jukebox jukebox;

    PickupObjects playerPickupObjs;
    bool changeMusicQuestActive = false;

    List<Quest> activeQuests = new List<Quest>();
    Dictionary<Quest, List<GameObject>> objectsToDestroyOnQuestFinish = new Dictionary<Quest, List<GameObject>>();

    Quest GetRandomizedQuest()
    {
        Transform randomNpc = NPCManager.Instance.NPCs[Random.Range(0, NPCManager.Instance.NPCs.Count)].transform;

        StateMachine stateMachine = randomNpc.gameObject.GetComponent<StateMachine>();

        int i = 0;

        while (stateMachine.CurrentState != NpcState.Happy)
        {
            randomNpc = NPCManager.Instance.NPCs[Random.Range(0, NPCManager.Instance.NPCs.Count)].transform;
            stateMachine = randomNpc.gameObject.GetComponent<StateMachine>();
            i++;

            if(i == 100)
            {
                //Debug.LogError("Everyone is propably leaving");
                return null;
            }
        }

        QuestType typeOfQuest = (QuestType)RandomExtension.ChooseFromMultipleWeighted(new List<int> { (int)QuestType.GetSomething,
            (int)QuestType.ChangeMusic, (int)QuestType.ThrowTheTrashOut, (int)QuestType.Puking }, new List<int> { 60, 10, 25, 5 });

        // Debug
        typeOfQuest = QuestType.Puking;
        //typeOfQuest = QuestType.ThrowTheTrashOut;
        //typeOfQuest = QuestType.GetSomething;

        QuestState npcActiveQuest = randomNpc.gameObject.GetComponent<QuestState>();

        while(typeOfQuest == QuestType.ChangeMusic && changeMusicQuestActive)
        {
            typeOfQuest = (QuestType)RandomExtension.ChooseFromMultipleWeighted(new List<int> { (int)QuestType.GetSomething,
                (int)QuestType.ChangeMusic, (int)QuestType.ThrowTheTrashOut, (int)QuestType.Puking }, new List<int> { 60, 10, 25, 5 });
        }

        if (npcActiveQuest.ActiveQuest != null) 
        {
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

        if (typeOfQuest == QuestType.GetSomething)
        {
            objType = (PickupableObjectType)RandomExtension.ChooseFromMultiple(new List<int> { (int)PickupableObjectType.Beer,
                (int)PickupableObjectType.Chips, (int)PickupableObjectType.Soda });
        }
        else if (typeOfQuest == QuestType.ThrowTheTrashOut)
        {
            objType = PickupableObjectType.Trash;
        }
        else if (typeOfQuest == QuestType.ChangeMusic)
        {
            changeMusicQuestActive = true;
        }

        return new Quest(typeOfQuest, timeBeforeQuestFail, objType, randomNpc);
    }

    void CreateNewQuest()
    {
        Coroutine c;
        Quest quest = GetRandomizedQuest();

        if (quest == null)
            return;

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
        else if(quest.TypeOfQuest == QuestType.ChangeMusic)
        {
            jukebox.QuestCreated(FinishQuest, quest);
        }

        if(quest.TypeOfQuest != QuestType.Puking)
        {
            quest.Npc.GetComponent<StateMachine>().ChangeState(NpcState.Waiting);
            quest.Npc.GetComponent<NPCAnimations>().ChangeAnimation(AnimationType.Waiting);
        }
        else
        {
            quest.Npc.GetComponent<StateMachine>().ChangeState(NpcState.Sick);
            quest.Npc.GetComponent<NPCAnimations>().ChangeAnimation(AnimationType.Puking);
        }

        quest.Npc.GetComponent<QuestState>().ActiveQuest = quest;

        //Debug.Log("New quest: " + quest.TypeOfQuest);
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
            objToDestroyLater.GetComponent<PickupableObjects>().AssignedQuest = quest;
        }

        objToDestroyLater = Instantiate(questMarkerPrefab, ui.transform);
        PointTowardsQuest questMarker = objToDestroyLater.GetComponent<PointTowardsQuest>();
        questMarker.questWorldTransform = quest.Npc;
        questMarker.SetSprite(quest.TypeOfQuest, quest.TypeToGet);
        objectsToDestroyOnQuestFinish[quest].Add(objToDestroyLater);
    }

    void FinishQuest(Quest finishedQuest)
    {
        if (finishedQuest == null)
            return;

        Debug.Log("Finished quest: " + finishedQuest.TypeOfQuest);

        if (finishedQuest.TypeOfQuest == QuestType.ChangeMusic)
        {
            jukebox.ResetQuest();
            changeMusicQuestActive = false;
        }

        finishedQuest.Npc.GetComponent<QuestState>().ActiveQuest.TypeOfQuest = QuestType.None;

        finishedQuest.Npc.GetComponent<StateMachine>().ChangeState(NpcState.Happy);

        TimeManager.Instance.StopCoroutine(finishedQuest.FailCoroutine);

        DestroyQuestObjects(finishedQuest);

        activeQuests.Remove(finishedQuest);

        finishedQuest.Npc.GetComponent<NPCAnimations>().ChangeAnimation(AnimationType.Dancing);

        ScoreManager.Instance.IncreaseScore();
    }

    void FailQuest(Quest questToFail)
    {
        Debug.Log("Failed quest: " + questToFail.TypeOfQuest);

        CreateQuestFailObjects(questToFail);

        DestroyQuestObjects(questToFail);

        activeQuests.Remove(questToFail);

        if(questToFail.Npc != null && questToFail.TypeOfQuest == QuestType.Puking)
        {
            questToFail.Npc.GetComponent<NPCAnimations>().ChangeAnimation(AnimationType.Dancing);
        }

        if (questToFail.Npc != null)
        {
            ChangeNpcsState(questToFail);
            QuestState qState = questToFail.Npc.GetComponent<QuestState>();
            if(qState.ActiveQuest != null)
                qState.ActiveQuest.TypeOfQuest = QuestType.None;
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

            if (playerPickupObjs.pickedUpRigidbody != null && playerPickupObjs.pickedUpRigidbody.gameObject.transform == quest.Npc)
            {
                playerPickupObjs.DropObject();
            }

            foreach (GameObject g in NPCManager.Instance.NPCs)
            {
                if(Vector3.Distance(quest.Npc.position, g.transform.position) <= pukeRange && g.transform != quest.Npc)
                {
                    if (playerPickupObjs.pickedUpRigidbody != null && playerPickupObjs.pickedUpRigidbody.gameObject == g)
                    {
                        playerPickupObjs.DropObject();
                    }

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

    void StopCoroutines()
    {
        TimeManager.Instance.StopAllCoroutines();
        LevelManager.Instance.OnBeforeSceneLoad -= StopAllCoroutines;
    }

    private void Awake()
    {
        Instance = this;
        //LevelManager.Instance.OnBeforeSceneLoad += StopAllCoroutines;
    }

    private void Start()
    {
        playerPickupObjs = FindObjectOfType<PickupObjects>();

        //TimeManager.Instance.StartTimer(2, CreateNewQuest);
        TimeManager.Instance.StartTimer(0, CreateNewQuest);
    }
}
