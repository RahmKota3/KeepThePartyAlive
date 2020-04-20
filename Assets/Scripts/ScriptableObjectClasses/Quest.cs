using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestType { None, GetSomething, ThrowTheTrashOut, ChangeMusic, Puking }

public class Quest
{
    public QuestType TypeOfQuest;
    public PickupableObjectType TypeToGet;

    public float TimeLimit = 15f;

    public Transform Npc;

    public Coroutine FailCoroutine;

    public Quest(QuestType type, float timeLimit, PickupableObjectType typeToGet, Transform npc)
    {
        TypeOfQuest = type;
        TimeLimit = timeLimit;
        TypeToGet = typeToGet;
        Npc = npc;
    }
}
