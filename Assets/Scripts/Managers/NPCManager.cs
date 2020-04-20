using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public static NPCManager Instance;

    public List<GameObject> NPCs;

    public void AddNpcToList(GameObject npc)
    {
        NPCs.Add(npc);
    }

    private void Awake()
    {
        Instance = this;
    }
}
