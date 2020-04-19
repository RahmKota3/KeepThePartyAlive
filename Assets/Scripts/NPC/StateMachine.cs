using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NpcState { Happy, Waiting, Angry }

public class StateMachine : MonoBehaviour
{
    NpcState currentState = NpcState.Happy;


}
