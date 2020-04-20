using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public static NPCManager Instance;

    public List<GameObject> NPCs;

    public void RemoveNpcFromList(GameObject npc)
    {
        NPCs.Remove(npc);
    }

    public void AddNpcToList(GameObject npc)
    {
        NPCs.Add(npc);
    }

    private void Awake()
    {
        Instance = this;
    }
}
