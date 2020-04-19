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

    List<Quest> activeQuests = new List<Quest>();
    Dictionary<Quest, GameObject> objectsToDestroyOnQuestFinish = new Dictionary<Quest, GameObject>();

    Quest GetRandomizedQuest()
    {
        QuestType typeOfQuest = (QuestType)RandomExtension.ChooseFromMultipleWeighted(new List<int> { (int)QuestType.GetSomething,
            (int)QuestType.ChangeMusic, (int)QuestType.Basketball }, new List<int> { 50, 30, 20 });

        return new Quest(typeOfQuest, timeBeforeQuestFail, 
            (PickupableObjectType.Beer));

        return new Quest(typeOfQuest, timeBeforeQuestFail,
            (PickupableObjectType)Random.Range(1, System.Enum.GetValues(typeof(PickupableObjectType)).Length));

        // Debug
        return new Quest(QuestType.GetSomething, timeBeforeQuestFail,
            PickupableObjectType.Beer);
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

        Debug.Log("New quest: " + quest.TypeOfQuest);
    }

    void CreateObjectsNeededForQuest(Quest quest)
    {
        if(quest.TypeOfQuest == QuestType.GetSomething)
        {
            GameObject trigger = Instantiate(itemReceiverPrefab, new Vector3(-101, 0, -23), Quaternion.identity);
            trigger.GetComponent<ObjectTrigger>().QuestCreated(FinishQuest, quest);
            objectsToDestroyOnQuestFinish[quest] = trigger;
        }
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
    }

    void DestroyQuestObjects(Quest quest)
    {
        if (objectsToDestroyOnQuestFinish.ContainsKey(quest))
            Destroy(objectsToDestroyOnQuestFinish[quest]);
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
