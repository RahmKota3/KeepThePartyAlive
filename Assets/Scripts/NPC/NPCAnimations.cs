using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimations : MonoBehaviour
{
    Animator anim;

    Dictionary<AnimationType, string> animationTriggers = new Dictionary<AnimationType, string>();

    public void ChangeAnimation(AnimationType animationToPlay)
    {
        if (animationToPlay != AnimationType.Walking)
            anim.applyRootMotion = true;
        else
            anim.applyRootMotion = false;

        anim.SetTrigger(animationTriggers[animationToPlay]);
    }

    private void Awake()
    {
        animationTriggers[AnimationType.Dancing] = "Dancing";
        animationTriggers[AnimationType.Walking] = "Exit";
        animationTriggers[AnimationType.Nervous] = "Quest_critical";
        animationTriggers[AnimationType.Waiting] = "Quest_active";
        animationTriggers[AnimationType.Puking] = "Vomit";
    }

    private void Start()
    {
        anim = GetComponent<RandomModelActivator>().ActiveAnimator;
    }
}
