using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private string currentAnimation;

    private void Awake()
    {
        if (anim == null) anim = GetComponent<Animator>();
    }

    public void SetAnimation(string animation)
    {
        if (currentAnimation != null) 
        {
            anim.SetBool(currentAnimation, false);
        }        

        if (animation == null) { return; }
        anim.SetBool(animation, true);
        currentAnimation = animation;
    }

    public void ResetAnimation(string animation)
    {
        anim.SetBool(animation, false);
    }
}
