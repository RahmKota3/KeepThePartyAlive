using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestType { None, GetSomething, Basketball, ChangeMusic }

public class Quest
{
    public QuestType TypeOfQuest;
    public PickupableObjectType TypeToGet;

    public float TimeLimit = 15f;

    public Coroutine FailCoroutine;

    public Quest(QuestType type, float timeLimit, PickupableObjectType typeToGet = PickupableObjectType.None)
    {
        TypeOfQuest = type;
        TimeLimit = timeLimit;
        TypeToGet = typeToGet;
    }
}
