using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NpcState { Happy, Waiting, Angry, Sick }

public class StateMachine : MonoBehaviour
{
    NpcState currentState = NpcState.Happy;

    public System.Action<NpcState> OnStateChanged;

    public void ChangeState(NpcState state)
    {
        currentState = state;
        OnStateChanged?.Invoke(state);
    }
}
