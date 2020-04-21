using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jukebox : MonoBehaviour
{
    bool isCollidingWithPlayer = false;

    System.Action<Quest> onFinish;
    Quest quest;

    public void QuestCreated(System.Action<Quest> onFinish, Quest quest)
    {
        this.onFinish = onFinish;
        this.quest = quest;
    }

    public void ResetQuest()
    {
        quest = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            isCollidingWithPlayer = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            isCollidingWithPlayer = false;
    }

    void ChangeMusic()
    {
        if (isCollidingWithPlayer == false)
            return;

        SoundsManager.Instance.ChangeSong();

        onFinish?.Invoke(quest);
    }

    private void Awake()
    {
        InputManager.Instance.OnPickUpButtonPressed += ChangeMusic;
    }
}
