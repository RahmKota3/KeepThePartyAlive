using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationType { Idle, Walking, PickingUp, Dancing, Drinking, Waiting, Nervous }

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] Animator anim;

    Dictionary<AnimationType, string> animationTriggerName = new Dictionary<AnimationType, string>();

    public void ActivateAnimation(AnimationType type)
    {
        if(anim == null)
        {
            //Debug.Log("No animator assigned.");
            return;
        }

        if(type == AnimationType.Walking)
        {
            SetAllAnimationBoolsToFalse();
            anim.SetBool(animationTriggerName[type], true);
        }
        else if(type == AnimationType.Idle)
        {
            SetAllAnimationBoolsToFalse();
            anim.SetBool(animationTriggerName[type], true);
        }
        else
        {
            SetAllAnimationBoolsToFalse();
            anim.SetTrigger(animationTriggerName[type]);
        }
    }

    void SetAllAnimationBoolsToFalse()
    {
        anim.SetBool(animationTriggerName[AnimationType.Idle], false);
        anim.SetBool(animationTriggerName[AnimationType.Walking], false);
    }

    void SetUpAnimationDictionary()
    {
        animationTriggerName[AnimationType.Idle] = "Idle";
        animationTriggerName[AnimationType.Walking] = "Walking";
        animationTriggerName[AnimationType.Dancing] = "Dancing";
        animationTriggerName[AnimationType.Waiting] = "Waiting";
        animationTriggerName[AnimationType.PickingUp] = "PickUp";
    }

    private void Awake()
    {
        SetUpAnimationDictionary();
    }
}
