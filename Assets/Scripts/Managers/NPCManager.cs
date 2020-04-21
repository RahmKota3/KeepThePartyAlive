using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public static NPCManager Instance;

    public List<GameObject> NPCs;

    public int? StartingNpcAmount = null;

    public System.Action<int> OnNpcAmountChanged;

    public void RemoveNpcFromList(GameObject npc)
    {
        NPCs.Remove(npc);

        OnNpcAmountChanged?.Invoke(NPCs.Count);

        if(NPCs.Count == 0)
        {
            if(LevelManager.Instance.CurrentScene == SceneType.Gameplay && Application.isPlaying)
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
        if(Input.GetKeyDown(KeyCode.F1) && Application.isPlaying && Application.isEditor)
            LevelManager.Instance.LoadScene(SceneType.LoseScreen);

        if (StartingNpcAmount.HasValue == false)
        {
            StartingNpcAmount = NPCs.Count;
            OnNpcAmountChanged?.Invoke(StartingNpcAmount.Value);
        }
    }
}
