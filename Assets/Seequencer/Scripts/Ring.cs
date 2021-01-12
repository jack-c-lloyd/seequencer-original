using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    [Range(float.Epsilon, 3.0f)] public float duration = 2.0f;

    public GvrReticlePointer reticlePointer;

    private Animator animator;

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private string getAnimationName()
    {
        foreach (AnimationClip animationClip in animator.runtimeAnimatorController.animationClips)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(animationClip.name))
            {
                return animationClip.name.ToString();
            }
        }

        return null;
    }

    public void PointerEnter()
    {
        animator.SetFloat("Duration", 1 / duration);
        animator.SetBool("Active", true);
        animator.Play(getAnimationName(), 0, 0); // faulty
    }

    public void PointerExit()
    {
        animator.Play(getAnimationName(), 0, 0); // faulty
        animator.SetBool("Active", false);
    }

    private void LateUpdate()
    {
        var interactable = reticlePointer.CurrentRaycastResult.gameObject;

        if (interactable != null)
        {
            var component = interactable.GetComponent<Interactable>();

            if (component != null)
            {
                animator.SetFloat("Duration", 1 / duration);
                animator.SetBool("Active", true);
                return;
            }
        }

        animator.Play(getAnimationName(), 0, 0); // faulty
        animator.SetBool("Active", false);
    }

    public void Interact()
    {
        var target = reticlePointer.CurrentRaycastResult.gameObject;

        if (target != null)
        {
            var interactable = target.GetComponent<Interactable>();

            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }
}
