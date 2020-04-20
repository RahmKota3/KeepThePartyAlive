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

    [Header("References")]
    [SerializeField] GameObject itemReceiverPrefab;
    [SerializeField] GameObject questMarkerPrefab;
    [SerializeField] GameObject trashPrefab;
    [SerializeField] ObjectTrigger trashcan;
    [SerializeField] GameObject ui;

    List<Quest> activeQuests = new List<Quest>();
    Dictionary<Quest, List<GameObject>> objectsToDestroyOnQuestFinish = new Dictionary<Quest, List<GameObject>>();

    Quest GetRandomizedQuest()
    {
        QuestType typeOfQuest = (QuestType)RandomExtension.ChooseFromMultipleWeighted(new List<int> { (int)QuestType.GetSomething,
            (int)QuestType.ChangeMusic, (int)QuestType.ThrowTheTrashOut }, new List<int> { 60, 10, 30 });

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

        return new Quest(typeOfQuest, timeBeforeQuestFail, objType);
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

        Debug.Log("New quest: " + quest.TypeOfQuest);
    }

    void CreateObjectsNeededForQuest(Quest quest)
    {
        Transform randomNpc = NPCManager.Instance.NPCs[Random.Range(0, NPCManager.Instance.NPCs.Count)].transform;
        GameObject objToDestroyLater;

        objectsToDestroyOnQuestFinish[quest] = new List<GameObject>();

        if(quest.TypeOfQuest == QuestType.GetSomething)
        {
            objToDestroyLater = Instantiate(itemReceiverPrefab, randomNpc.position, Quaternion.identity);
            objToDestroyLater.GetComponent<ObjectTrigger>().QuestCreated(FinishQuest, quest);
            objectsToDestroyOnQuestFinish[quest].Add(objToDestroyLater);
        }
        else if(quest.TypeOfQuest == QuestType.ThrowTheTrashOut)
        {
            objToDestroyLater = Instantiate(trashPrefab, randomNpc.Find("TrashSpawnPosition").position, Quaternion.identity);
            objectsToDestroyOnQuestFinish[quest].Add(objToDestroyLater);
        }

        // Create quest marker
        objToDestroyLater = Instantiate(questMarkerPrefab, ui.transform);
        objToDestroyLater.GetComponent<PointTowardsQuest>().questWorldPosition = randomNpc.position + new Vector3(0, 2.5f, 0);
        objectsToDestroyOnQuestFinish[quest].Add(objToDestroyLater);
    }

    void FinishQuest(Quest finishedQuest)
    {
        Debug.Log("Finished quest: " + finishedQuest.TypeOfQuest);

        TimeManager.Instance.StopCoroutine(finishedQuest.FailCoroutine);

        DestroyQuestObjects(finishedQuest);

        activeQuests.Remove(finishedQuest);
        // TODO: Add score.
    }

    void FailQuest(Quest questToFail)
    {
        Debug.Log("Failed quest: " + questToFail.TypeOfQuest);

        DestroyQuestObjects(questToFail);

        activeQuests.Remove(questToFail);
        // TODO: Remove score.
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
        //TimeManager.Instance.StartTimer(2, CreateNewQuest);
        TimeManager.Instance.StartTimer(0, CreateNewQuest);
    }
}
