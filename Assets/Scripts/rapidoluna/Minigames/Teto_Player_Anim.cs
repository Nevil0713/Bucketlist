using UnityEngine;

public class Teto_Player_Anim : MonoBehaviour
{
    Animator animator;

    public void HappyTrigger()
    {
        animator.SetTrigger("isHappy");
    }

    public void HappyStop()
    {
        animator.SetTrigger("noHappy");
    }

    public void SadTrigger()
    {
        animator.SetTrigger("isSad");
    }

    public void SadStop()
    {
        animator.SetTrigger("noSad");
    }

    public void OhTrigger()
    {
        animator.SetTrigger("isYeah");
    }

    public void OhStop()
    {
        animator.SetTrigger("noYeah");
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
}
