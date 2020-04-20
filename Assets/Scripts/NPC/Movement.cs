using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    [SerializeField] Transform houseEnterance;
    [SerializeField] SphereCollider pickupTrigger;

    NavMeshAgent agent;
    NavMeshObstacle obstacle;
    NPCAnimations animator;
    StateMachine stateMachine;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enterance" && stateMachine.CurrentState == NpcState.Angry)
        {
            ScoreManager.Instance.DecreaseScore();
            Destroy(this.gameObject);
        }
    }

    void FleeTheParty(NpcState state)
    {
        if (state != NpcState.Angry)
            return;

        pickupTrigger.enabled = false;
        obstacle.enabled = false;
        agent.enabled = true;
        agent.SetDestination(houseEnterance.position);
        animator.ChangeAnimation(AnimationType.Walking);
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        obstacle = GetComponent<NavMeshObstacle>();
        animator = GetComponent<NPCAnimations>();
        stateMachine = GetComponent<StateMachine>();

        GetComponent<StateMachine>().OnStateChanged += FleeTheParty;
    }

    private void OnDestroy()
    {
        NPCManager.Instance.RemoveNpcFromList(this.gameObject);
    }
}
