using UnityEngine;

public class Teto_Player_Anim : MonoBehaviour
{
    Animator animator;

    public void PlayHappy(float duration = 0.25f)
    {
        if (!animator.GetBool("isHappy"))
        {
            animator.SetBool("isHappy", true);
            Invoke(nameof(StopHappy), duration);
        }
    }

    public void StopHappy()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("happy"))
        {
            animator.SetBool("isHappy", false);
        }
    }

    public void SetSad(bool value)
    {
        animator.SetBool("isSad", value);
    }

    public void SetYeah(bool value)
    {
        animator.SetBool("isYeah", value);
    }

    public void SetShock(bool value)
    {
        animator.SetBool("isShock", value);
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
}

