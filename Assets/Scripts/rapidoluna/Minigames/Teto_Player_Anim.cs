using UnityEngine;

public class Teto_Player_Anim : MonoBehaviour
{
    Animator animator;

    public void PlayHappy(float duration = 0.25f)
    {
        animator.SetBool("isHappy", true);
        Invoke(nameof(StopHappy), duration);
    }

    void StopHappy()
    {
        animator.SetBool("isHappy", false);
    }

    // Sad ���� ����
    public void SetSad(bool value)
    {
        animator.SetBool("isSad", value);
    }

    // Oh(�¸�) ���� ����
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

