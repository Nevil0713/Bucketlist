using UnityEngine;

public class Player_anim : MonoBehaviour
{
    Animator animator;
    
    void Start()
    {
        animator.SetBool("isHappy", false);
        animator.SetBool("isSad", false);
    }

    
    void Update()
    {
        
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void HappyTrigger()
    {
        animator.SetBool("isHappy", true);
    }

    void SadTrigger()
    {
        animator.SetBool("isSad", true);
    }

}
