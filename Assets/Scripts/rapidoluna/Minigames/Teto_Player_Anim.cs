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

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
}
