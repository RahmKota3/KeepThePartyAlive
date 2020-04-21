using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_GuestsLeft : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI guestsLeft;

    void UpdateText(int amount)
    {
        guestsLeft.text = "Guests left: " + amount + "/" + NPCManager.Instance.StartingNpcAmount;
    }

    private void Start()
    {
        NPCManager.Instance.OnNpcAmountChanged += UpdateText;
    }
}
