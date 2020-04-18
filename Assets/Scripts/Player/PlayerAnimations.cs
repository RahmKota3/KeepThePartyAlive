using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationType { Walking, PickingUp, Dancing, Drinking, Waiting, Nervous }

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
            anim.SetBool(animationTriggerName[type], true);
        }
        else
        {
            anim.SetBool(animationTriggerName[AnimationType.Walking], false);
            anim.SetTrigger(animationTriggerName[type]);
        }
    }

    void SetUpAnimationDictionary()
    {
        animationTriggerName[AnimationType.Walking] = "Walking";
        animationTriggerName[AnimationType.Dancing] = "Dancing";
        animationTriggerName[AnimationType.Waiting] = "Waiting";
    }

    private void Awake()
    {
        SetUpAnimationDictionary();
    }
}
