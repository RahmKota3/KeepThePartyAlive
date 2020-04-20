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

        if(NPCs.Count == 0)
        {
            if(LevelManager.Instance.CurrentScene == SceneType.Gameplay)
                LevelManager.Instance.LoadScene(SceneType.LoseScreen);
        }
    }

    public void AddNpcToList(GameObject npc)
    {
        NPCs.Add(npc);
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
            LevelManager.Instance.LoadScene(SceneType.LoseScreen);
    }
}
